namespace BookPromoTracker.Dtos.Prices;

public class PriceHistoryItemResponse
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public DateTime CheckedAt { get; set; }
}
