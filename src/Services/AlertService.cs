using BookPromoTracker.Data;
using BookPromoTracker.Dtos.Alerts;
using Microsoft.EntityFrameworkCore;

namespace BookPromoTracker.Services;

public class AlertService(AppDbContext db) : IAlertService
{
    public async Task<IReadOnlyList<AlertResponse>> ListAsync(
        bool? unreadOnly,
        Guid? bookId,
        CancellationToken cancellationToken = default
    )
    {
        var query = db.Alerts.AsNoTracking();

        if (unreadOnly == true)
        {
            query = query.Where(alert => !alert.WasRead);
        }

        if (bookId.HasValue)
        {
            query = query.Where(alert => alert.BookId == bookId.Value);
        }

        return await query
            .OrderByDescending(alert => alert.CreatedAt)
            .Select(alert => new AlertResponse
            {
                Id = alert.Id,
                BookId = alert.BookId,
                BookTitle = alert.Book.Title,
                CurrentPrice = alert.CurrentPrice,
                TargetPrice = alert.TargetPrice,
                Message = alert.Message,
                WasRead = alert.WasRead,
                CreatedAt = alert.CreatedAt,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> MarkAsReadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var alert = await db.Alerts.FindAsync([id], cancellationToken);

        if (alert is null)
        {
            return false;
        }

        alert.WasRead = true;
        await db.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> MarkAsUnreadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var alert = await db.Alerts.FindAsync([id], cancellationToken);

        if (alert is null)
        {
            return false;
        }

        alert.WasRead = false;
        await db.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<int> GetUnreadCountAsync(CancellationToken cancellationToken = default)
    {
        return await db.Alerts
            .AsNoTracking()
            .CountAsync(alert => !alert.WasRead, cancellationToken);
    }
}
