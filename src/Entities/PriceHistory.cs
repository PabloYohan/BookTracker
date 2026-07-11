namespace BookPromoTracker.Entities;

public class PriceHistory
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public DateTime CheckedAt { get; set; }

    public Book Book { get; set; } = null!;
}