namespace BookPromoTracker.Api.Entities;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public string Asin { get; set; } = string.Empty;
    public string ProductUrl { get; set; } = string.Empty;
    public decimal TargetPrice { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
