using BookPromoTracker.Dtos.Prices;

namespace BookPromoTracker.Services;

public interface IPriceHistoryService
{
    Task<(
        ManualPriceRegistrationResponse? Result,
        Dictionary<string, string[]>? ValidationErrors,
        bool BookNotFound,
        bool BookInactive
    )> RegisterManualPriceAsync(
        Guid bookId,
        RegisterManualPriceRequest request,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<PriceHistoryItemResponse>> GetPriceHistoryAsync(
        Guid bookId,
        CancellationToken cancellationToken = default
    );

    Task<(LowestPriceResponse? Result, bool BookExists)> GetLowestPriceAsync(
        Guid bookId,
        CancellationToken cancellationToken = default
    );
}
