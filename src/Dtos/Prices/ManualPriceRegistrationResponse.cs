namespace BookPromoTracker.Dtos.Prices;

public class ManualPriceRegistrationResponse
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public DateTime CheckedAt { get; set; }
    public decimal TargetPrice { get; set; }
    public bool TargetReached { get; set; }
    public bool AlertCreated { get; set; }
}
