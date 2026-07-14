namespace BookPromoTracker.Dtos.Alerts;

public class AlertResponse
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public decimal CurrentPrice { get; set; }
    public decimal TargetPrice { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool WasRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
