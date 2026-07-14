using BookPromoTracker.Services;

namespace BookPromoTracker.Tests;

public class AmazonProductUrlParserTests
{
    private readonly AmazonProductUrlParser _parser = new();

    [Theory]
    [InlineData("https://www.amazon.com.br/dp/8532520709", "8532520709")]
    [InlineData(
        "https://www.amazon.com.br/Livro/dp/8532520709?ref_=abc",
        "8532520709"
    )]
    [InlineData("https://www.amazon.com.br/gp/product/B0ABC12345", "B0ABC12345")]
    [InlineData("https://m.amazon.com.br/dp/B0ABC12345", "B0ABC12345")]
    public void TryParse_ValidUrls_ReturnsCanonicalUrl(string url, string expectedAsin)
    {
        var success = _parser.TryParse(url, out var result, out var error);

        Assert.True(success);
        Assert.Null(error);
        Assert.NotNull(result);
        Assert.Equal(expectedAsin, result!.Asin);
        Assert.Equal($"https://www.amazon.com.br/dp/{expectedAsin}", result.CanonicalUrl);
    }

    [Theory]
    [InlineData("https://www.amazon.com/dp/8532520709")]
    [InlineData("https://amazon.com.br.exemplo.com/dp/8532520709")]
    [InlineData("https://www.amazon.com.br/produto-sem-asin")]
    [InlineData("http://www.amazon.com.br/dp/8532520709")]
    [InlineData("texto qualquer")]
    [InlineData("https://amzn.to/exemplo")]
    public void TryParse_InvalidUrls_ReturnsError(string url)
    {
        var success = _parser.TryParse(url, out var result, out var error);

        Assert.False(success);
        Assert.Null(result);
        Assert.NotNull(error);
        Assert.Contains("Amazon Brasil", error);
    }
}
