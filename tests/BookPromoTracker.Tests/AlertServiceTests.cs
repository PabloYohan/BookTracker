using BookPromoTracker.Entities;
using BookPromoTracker.Services;

namespace BookPromoTracker.Tests;

public class AlertServiceTests : SqliteTestBase
{
    private AlertService CreateService() => new(Db);

    private Alert CreateAlert(Guid bookId, bool wasRead = false, decimal currentPrice = 29.90m)
    {
        var alert = new Alert
        {
            Id = Guid.NewGuid(),
            BookId = bookId,
            CurrentPrice = currentPrice,
            TargetPrice = 35m,
            Message = "Alerta de teste",
            WasRead = wasRead,
            CreatedAt = DateTime.UtcNow,
        };

        Db.Alerts.Add(alert);
        Db.SaveChanges();

        return alert;
    }

    [Fact]
    public async Task ListAsync_UnreadOnlyFilter_ReturnsOnlyUnreadAlerts()
    {
        var book = CreateBook();
        CreateAlert(book.Id, wasRead: false);
        CreateAlert(book.Id, wasRead: true);
        var service = CreateService();

        var alerts = await service.ListAsync(unreadOnly: true, bookId: null);

        Assert.Single(alerts);
        Assert.False(alerts[0].WasRead);
    }

    [Fact]
    public async Task MarkAsReadAsync_ExistingAlert_UpdatesWasRead()
    {
        var book = CreateBook();
        var alert = CreateAlert(book.Id);
        var service = CreateService();

        var updated = await service.MarkAsReadAsync(alert.Id);

        Assert.True(updated);
        Assert.True(Db.Alerts.Single().WasRead);
    }

    [Fact]
    public async Task MarkAsUnreadAsync_ExistingAlert_UpdatesWasRead()
    {
        var book = CreateBook();
        var alert = CreateAlert(book.Id, wasRead: true);
        var service = CreateService();

        var updated = await service.MarkAsUnreadAsync(alert.Id);

        Assert.True(updated);
        Assert.False(Db.Alerts.Single().WasRead);
    }

    [Fact]
    public async Task GetUnreadCountAsync_ReturnsUnreadTotal()
    {
        var book = CreateBook();
        CreateAlert(book.Id, wasRead: false);
        CreateAlert(book.Id, wasRead: false);
        CreateAlert(book.Id, wasRead: true);
        var service = CreateService();

        var count = await service.GetUnreadCountAsync();

        Assert.Equal(2, count);
    }
}
