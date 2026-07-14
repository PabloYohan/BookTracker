namespace BookPromoTracker.Dtos.Books;

public class CreateBookRequest
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public string ProductUrl { get; set; } = string.Empty;
    public decimal TargetPrice { get; set; }
}
