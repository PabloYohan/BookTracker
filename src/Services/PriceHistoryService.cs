using BookPromoTracker.Data;
using BookPromoTracker.Dtos.Prices;
using BookPromoTracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookPromoTracker.Services;

public class PriceHistoryService(AppDbContext db, ILogger<PriceHistoryService> logger)
    : IPriceHistoryService
{
    public const string ManualSource = "Manual - Amazon";
    public const string DefaultCurrency = "BRL";
    private static readonly TimeSpan MaxFutureObservedAt = TimeSpan.FromMinutes(5);

    public async Task<(
        ManualPriceRegistrationResponse? Result,
        Dictionary<string, string[]>? ValidationErrors,
        bool BookNotFound,
        bool BookInactive
    )> RegisterManualPriceAsync(
        Guid bookId,
        RegisterManualPriceRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationErrors = ValidateRequest(request);

        if (validationErrors is not null)
        {
            return (null, validationErrors, false, false);
        }

        var book = await db.Books.FindAsync([bookId], cancellationToken);

        if (book is null)
        {
            logger.LogWarning(
                "Tentativa de registrar preço manual para livro inexistente {BookId}",
                bookId
            );

            return (null, null, true, false);
        }

        if (!book.IsActive)
        {
            return (null, null, false, true);
        }

        var currency = string.IsNullOrWhiteSpace(request.Currency)
            ? DefaultCurrency
            : request.Currency.Trim().ToUpperInvariant();

        var observedAt = request.ObservedAt ?? DateTime.UtcNow;
        var price = request.Price;

        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var previousPrice = await db.PriceHistories
                .AsNoTracking()
                .Where(history => history.BookId == bookId)
                .OrderByDescending(history => history.CheckedAt)
                .Select(history => (decimal?)history.Price)
                .FirstOrDefaultAsync(cancellationToken);

            var mostRecentAlertPrice = await db.Alerts
                .AsNoTracking()
                .Where(alert => alert.BookId == bookId)
                .OrderByDescending(alert => alert.CreatedAt)
                .Select(alert => (decimal?)alert.CurrentPrice)
                .FirstOrDefaultAsync(cancellationToken);

            var history = new PriceHistory
            {
                Id = Guid.NewGuid(),
                BookId = book.Id,
                Price = price,
                Currency = currency,
                Source = ManualSource,
                CheckedAt = observedAt,
            };

            db.PriceHistories.Add(history);

            var targetReached = price <= book.TargetPrice;
            var alertCreated = false;

            if (targetReached && ShouldCreateAlert(price, book.TargetPrice, previousPrice, mostRecentAlertPrice))
            {
                var alert = CreateAlert(book, price);
                db.Alerts.Add(alert);
                alertCreated = true;

                logger.LogInformation(
                    "Preço-alvo atingido para o livro {BookId}. Alerta criado: {AlertId}",
                    book.Id,
                    alert.Id
                );
            }

            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            logger.LogInformation(
                "Preço manual registrado para o livro {BookId}: {Price} {Currency}",
                book.Id,
                price,
                currency
            );

            return (
                new ManualPriceRegistrationResponse
                {
                    Id = history.Id,
                    BookId = book.Id,
                    BookTitle = book.Title,
                    Price = history.Price,
                    Currency = history.Currency,
                    Source = history.Source,
                    CheckedAt = history.CheckedAt,
                    TargetPrice = book.TargetPrice,
                    TargetReached = targetReached,
                    AlertCreated = alertCreated,
                },
                null,
                false,
                false
            );
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<IReadOnlyList<PriceHistoryItemResponse>> GetPriceHistoryAsync(
        Guid bookId,
        CancellationToken cancellationToken = default
    )
    {
        return await db.PriceHistories
            .AsNoTracking()
            .Where(history => history.BookId == bookId)
            .OrderByDescending(history => history.CheckedAt)
            .Select(history => new PriceHistoryItemResponse
            {
                Id = history.Id,
                Price = history.Price,
                Currency = history.Currency,
                Source = history.Source,
                CheckedAt = history.CheckedAt,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<(LowestPriceResponse? Result, bool BookExists)> GetLowestPriceAsync(
        Guid bookId,
        CancellationToken cancellationToken = default
    )
    {
        var bookExists = await db.Books
            .AsNoTracking()
            .AnyAsync(book => book.Id == bookId, cancellationToken);

        if (!bookExists)
        {
            return (null, false);
        }

        var lowest = await db.PriceHistories
            .AsNoTracking()
            .Where(history => history.BookId == bookId)
            .OrderBy(history => history.Price)
            .ThenByDescending(history => history.CheckedAt)
            .Select(history => new { history.Price, history.CheckedAt })
            .FirstOrDefaultAsync(cancellationToken);

        return (
            new LowestPriceResponse
            {
                BookId = bookId,
                LowestPrice = lowest?.Price,
                CheckedAt = lowest?.CheckedAt,
            },
            true
        );
    }

    public static bool ShouldCreateAlert(
        decimal newPrice,
        decimal targetPrice,
        decimal? previousPrice,
        decimal? mostRecentAlertPrice
    )
    {
        if (newPrice > targetPrice)
        {
            return false;
        }

        if (previousPrice.HasValue && previousPrice.Value == newPrice)
        {
            return false;
        }

        if (!previousPrice.HasValue)
        {
            return true;
        }

        if (previousPrice > targetPrice)
        {
            return true;
        }

        return mostRecentAlertPrice.HasValue && newPrice < mostRecentAlertPrice.Value;
    }

    private static Alert CreateAlert(Book book, decimal currentPrice)
    {
        var formattedCurrentPrice = currentPrice.ToString("F2");
        var formattedTargetPrice = book.TargetPrice.ToString("F2");

        return new Alert
        {
            Id = Guid.NewGuid(),
            BookId = book.Id,
            CurrentPrice = currentPrice,
            TargetPrice = book.TargetPrice,
            Message =
                $"O livro \"{book.Title}\" atingiu o preço desejado. "
                + $"Preço atual: R$ {formattedCurrentPrice}. "
                + $"Preço desejado: R$ {formattedTargetPrice}.",
            WasRead = false,
            CreatedAt = DateTime.UtcNow,
        };
    }

    private static Dictionary<string, string[]>? ValidateRequest(RegisterManualPriceRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        if (request.Price <= 0)
        {
            errors["price"] = ["O preço deve ser maior que zero."];
        }

        if (!string.IsNullOrWhiteSpace(request.Currency)
            && !string.Equals(request.Currency.Trim(), DefaultCurrency, StringComparison.OrdinalIgnoreCase))
        {
            errors["currency"] = ["Somente a moeda BRL é aceita nesta versão."];
        }

        if (request.ObservedAt.HasValue
            && request.ObservedAt.Value > DateTime.UtcNow.Add(MaxFutureObservedAt))
        {
            errors["observedAt"] = ["A data informada não pode ser excessivamente futura."];
        }

        return errors.Count > 0 ? errors : null;
    }
}
