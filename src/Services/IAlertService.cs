using BookPromoTracker.Dtos.Alerts;

namespace BookPromoTracker.Services;

public interface IAlertService
{
    Task<IReadOnlyList<AlertResponse>> ListAsync(
        bool? unreadOnly,
        Guid? bookId,
        CancellationToken cancellationToken = default
    );

    Task<bool> MarkAsReadAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> MarkAsUnreadAsync(Guid id, CancellationToken cancellationToken = default);

    Task<int> GetUnreadCountAsync(CancellationToken cancellationToken = default);
}
