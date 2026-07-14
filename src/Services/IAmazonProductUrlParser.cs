namespace BookPromoTracker.Services;

public sealed record AmazonProductUrlParseResult(string Asin, string CanonicalUrl);

public interface IAmazonProductUrlParser
{
    bool TryParse(string? url, out AmazonProductUrlParseResult? result, out string? errorMessage);
}
