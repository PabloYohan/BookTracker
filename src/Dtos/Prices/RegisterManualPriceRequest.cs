namespace BookPromoTracker.Dtos.Prices;

public class RegisterManualPriceRequest
{
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime? ObservedAt { get; set; }
}
