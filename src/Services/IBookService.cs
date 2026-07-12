using BookPromoTracker.Dtos.Books;

namespace BookPromoTracker.Services;

public interface IBookService
{
    Task<IReadOnlyList<BookListItemResponse>> ListAsync(CancellationToken cancellationToken = default);
    Task<BookResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(BookResponse? Book, Dictionary<string, string[]>? Errors)> CreateAsync(
        CreateBookRequest request,
        CancellationToken cancellationToken = default
    );
    Task<(BookResponse? Book, Dictionary<string, string[]>? Errors)> UpdateAsync(
        Guid id,
        UpdateBookRequest request,
        CancellationToken cancellationToken = default
    );
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BookResponse?> SetMonitoringAsync(
        Guid id,
        bool isActive,
        CancellationToken cancellationToken = default
    );
}
