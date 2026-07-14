namespace BookPromoTracker.Dtos.Prices;

public class LowestPriceResponse
{
    public Guid BookId { get; set; }
    public decimal? LowestPrice { get; set; }
    public DateTime? CheckedAt { get; set; }
}
