namespace BookPromoTracker.Entities;

public class Alert
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal TargetPrice { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool WasRead { get; set; }
    public DateTime CreatedAt { get; set; }

    public Book Book { get; set; } = null!;
}
