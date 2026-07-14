using BookPromoTracker.Dtos.Prices;
using BookPromoTracker.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace BookPromoTracker.Tests;

public class PriceHistoryServiceTests : SqliteTestBase
{
    private PriceHistoryService CreateService() =>
        new(Db, NullLogger<PriceHistoryService>.Instance);

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task RegisterManualPriceAsync_InvalidPrice_ReturnsValidationError(decimal price)
    {
        var book = CreateBook();
        var service = CreateService();

        var (result, errors, bookNotFound, bookInactive) =
            await service.RegisterManualPriceAsync(
                book.Id,
                new RegisterManualPriceRequest { Price = price }
            );

        Assert.Null(result);
        Assert.NotNull(errors);
        Assert.Contains("price", errors!.Keys);
        Assert.False(bookNotFound);
        Assert.False(bookInactive);
    }

    [Fact]
    public async Task RegisterManualPriceAsync_NonBrlCurrency_ReturnsValidationError()
    {
        var book = CreateBook();
        var service = CreateService();

        var (result, errors, _, _) = await service.RegisterManualPriceAsync(
            book.Id,
            new RegisterManualPriceRequest { Price = 29.90m, Currency = "USD" }
        );

        Assert.Null(result);
        Assert.NotNull(errors);
        Assert.Contains("currency", errors!.Keys);
    }

    [Fact]
    public async Task RegisterManualPriceAsync_BookNotFound_ReturnsNotFound()
    {
        var service = CreateService();

        var (result, errors, bookNotFound, bookInactive) =
            await service.RegisterManualPriceAsync(
                Guid.NewGuid(),
                new RegisterManualPriceRequest { Price = 29.90m }
            );

        Assert.Null(result);
        Assert.Null(errors);
        Assert.True(bookNotFound);
        Assert.False(bookInactive);
    }

    [Fact]
    public async Task RegisterManualPriceAsync_InactiveBook_ReturnsInactive()
    {
        var book = CreateBook(isActive: false);
        var service = CreateService();

        var (result, errors, bookNotFound, bookInactive) =
            await service.RegisterManualPriceAsync(
                book.Id,
                new RegisterManualPriceRequest { Price = 29.90m }
            );

        Assert.Null(result);
        Assert.Null(errors);
        Assert.False(bookNotFound);
        Assert.True(bookInactive);
    }

    [Fact]
    public async Task RegisterManualPriceAsync_SavesHistoryWithManualSource()
    {
        var book = CreateBook(targetPrice: 35m);
        var service = CreateService();

        var (result, _, _, _) = await service.RegisterManualPriceAsync(
            book.Id,
            new RegisterManualPriceRequest { Price = 40m }
        );

        Assert.NotNull(result);
        Assert.Equal(PriceHistoryService.ManualSource, result!.Source);
        Assert.Equal("BRL", result.Currency);
        Assert.False(result.TargetReached);
        Assert.False(result.AlertCreated);

        var history = Db.PriceHistories.Single();
        Assert.Equal(40m, history.Price);
        Assert.Equal(PriceHistoryService.ManualSource, history.Source);
    }

    [Fact]
    public async Task RegisterManualPriceAsync_FirstPriceBelowTarget_CreatesAlert()
    {
        var book = CreateBook(targetPrice: 35m);
        var service = CreateService();

        var (result, _, _, _) = await service.RegisterManualPriceAsync(
            book.Id,
            new RegisterManualPriceRequest { Price = 29.90m }
        );

        Assert.NotNull(result);
        Assert.True(result!.TargetReached);
        Assert.True(result.AlertCreated);

        var alert = Db.Alerts.Single();
        Assert.Equal(29.90m, alert.CurrentPrice);
        Assert.Equal(35m, alert.TargetPrice);
        Assert.False(alert.WasRead);
    }

    [Fact]
    public async Task RegisterManualPriceAsync_CrossingTarget_CreatesAlert()
    {
        var book = CreateBook(targetPrice: 40m);
        var service = CreateService();

        await service.RegisterManualPriceAsync(
            book.Id,
            new RegisterManualPriceRequest { Price = 45m }
        );

        var (result, _, _, _) = await service.RegisterManualPriceAsync(
            book.Id,
            new RegisterManualPriceRequest { Price = 39.90m }
        );

        Assert.NotNull(result);
        Assert.True(result!.AlertCreated);
        Assert.Single(Db.Alerts);
    }

    [Fact]
    public async Task RegisterManualPriceAsync_RepeatedPriceBelowTarget_DoesNotDuplicateAlert()
    {
        var book = CreateBook(targetPrice: 35m);
        var service = CreateService();

        await service.RegisterManualPriceAsync(
            book.Id,
            new RegisterManualPriceRequest { Price = 29.90m }
        );

        var (result, _, _, _) = await service.RegisterManualPriceAsync(
            book.Id,
            new RegisterManualPriceRequest { Price = 29.90m }
        );

        Assert.NotNull(result);
        Assert.False(result!.AlertCreated);
        Assert.Single(Db.Alerts);
        Assert.Equal(2, Db.PriceHistories.Count());
    }

    [Fact]
    public async Task RegisterManualPriceAsync_NewLowerPriceBelowTarget_CreatesAnotherAlert()
    {
        var book = CreateBook(targetPrice: 40m);
        var service = CreateService();

        await service.RegisterManualPriceAsync(
            book.Id,
            new RegisterManualPriceRequest { Price = 39.90m }
        );

        var (result, _, _, _) = await service.RegisterManualPriceAsync(
            book.Id,
            new RegisterManualPriceRequest { Price = 34.90m }
        );

        Assert.NotNull(result);
        Assert.True(result!.AlertCreated);
        Assert.Equal(2, Db.Alerts.Count());
    }
}
