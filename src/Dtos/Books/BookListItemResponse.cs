namespace BookPromoTracker.Dtos.Books;

public class BookListItemResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Asin { get; set; } = string.Empty;
    public string ProductUrl { get; set; } = string.Empty;
    public decimal TargetPrice { get; set; }
    public decimal? LastPrice { get; set; }
    public bool IsActive { get; set; }
}
