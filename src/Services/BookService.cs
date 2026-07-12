using BookPromoTracker.Data;
using BookPromoTracker.Dtos.Books;
using BookPromoTracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookPromoTracker.Services;

public class BookService(AppDbContext db) : IBookService
{
    public async Task<IReadOnlyList<BookListItemResponse>> ListAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await db.Books
            .AsNoTracking()
            .OrderBy(book => book.Title)
            .Select(book => new BookListItemResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                TargetPrice = book.TargetPrice,
                LastPrice = book.PriceHistories
                    .OrderByDescending(history => history.CheckedAt)
                    .Select(history => (decimal?)history.Price)
                    .FirstOrDefault(),
                IsActive = book.IsActive,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<BookResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var book = await db.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

        return book is null ? null : MapToResponse(book);
    }

    public async Task<(BookResponse? Book, Dictionary<string, string[]>? Errors)> CreateAsync(
        CreateBookRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var errors = ValidateCreateRequest(request);

        if (errors is not null)
        {
            return (null, errors);
        }

        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Author = request.Author.Trim(),
            Isbn = request.Isbn.Trim(),
            Asin = request.Asin.Trim(),
            ProductUrl = request.ProductUrl.Trim(),
            TargetPrice = request.TargetPrice,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
        };

        db.Books.Add(book);
        await db.SaveChangesAsync(cancellationToken);

        return (MapToResponse(book), null);
    }

    public async Task<(BookResponse? Book, Dictionary<string, string[]>? Errors)> UpdateAsync(
        Guid id,
        UpdateBookRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var errors = ValidateUpdateRequest(request);

        if (errors is not null)
        {
            return (null, errors);
        }

        var book = await db.Books.FindAsync([id], cancellationToken);

        if (book is null)
        {
            return (null, null);
        }

        book.Title = request.Title.Trim();
        book.Author = request.Author.Trim();
        book.Isbn = request.Isbn.Trim();
        book.Asin = request.Asin.Trim();
        book.ProductUrl = request.ProductUrl.Trim();
        book.TargetPrice = request.TargetPrice;
        book.IsActive = request.IsActive;

        await db.SaveChangesAsync(cancellationToken);

        return (MapToResponse(book), null);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var book = await db.Books.FindAsync([id], cancellationToken);

        if (book is null)
        {
            return false;
        }

        db.Books.Remove(book);
        await db.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<BookResponse?> SetMonitoringAsync(
        Guid id,
        bool isActive,
        CancellationToken cancellationToken = default
    )
    {
        var book = await db.Books.FindAsync([id], cancellationToken);

        if (book is null)
        {
            return null;
        }

        book.IsActive = isActive;
        await db.SaveChangesAsync(cancellationToken);

        return MapToResponse(book);
    }

    private static BookResponse MapToResponse(Book book) =>
        new()
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Isbn = book.Isbn,
            Asin = book.Asin,
            ProductUrl = book.ProductUrl,
            TargetPrice = book.TargetPrice,
            IsActive = book.IsActive,
            CreatedAt = book.CreatedAt,
        };

    private static Dictionary<string, string[]>? ValidateCreateRequest(CreateBookRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            errors["title"] = ["O título é obrigatório."];
        }

        if (string.IsNullOrWhiteSpace(request.Author))
        {
            errors["author"] = ["O autor é obrigatório."];
        }

        if (string.IsNullOrWhiteSpace(request.ProductUrl))
        {
            errors["productUrl"] = ["A URL do produto é obrigatória."];
        }

        if (request.TargetPrice <= 0)
        {
            errors["targetPrice"] = ["O preço desejado deve ser maior que zero."];
        }

        return errors.Count > 0 ? errors : null;
    }

    private static Dictionary<string, string[]>? ValidateUpdateRequest(UpdateBookRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            errors["title"] = ["O título é obrigatório."];
        }

        if (string.IsNullOrWhiteSpace(request.Author))
        {
            errors["author"] = ["O autor é obrigatório."];
        }

        if (string.IsNullOrWhiteSpace(request.ProductUrl))
        {
            errors["productUrl"] = ["A URL do produto é obrigatória."];
        }

        if (request.TargetPrice <= 0)
        {
            errors["targetPrice"] = ["O preço desejado deve ser maior que zero."];
        }

        return errors.Count > 0 ? errors : null;
    }
}
