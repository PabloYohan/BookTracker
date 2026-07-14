using System.Text.RegularExpressions;

namespace BookPromoTracker.Services;

public sealed partial class AmazonProductUrlParser : IAmazonProductUrlParser
{
    private const string ValidationErrorMessage =
        "A URL deve pertencer à Amazon Brasil e conter um ASIN válido. "
        + "Copie a URL completa do produto em amazon.com.br.";

    public bool TryParse(
        string? url,
        out AmazonProductUrlParseResult? result,
        out string? errorMessage
    )
    {
        result = null;
        errorMessage = null;

        if (string.IsNullOrWhiteSpace(url))
        {
            errorMessage = ValidationErrorMessage;
            return false;
        }

        if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri))
        {
            errorMessage = ValidationErrorMessage;
            return false;
        }

        if (!string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
        {
            errorMessage = ValidationErrorMessage;
            return false;
        }

        if (!IsAllowedAmazonBrazilHost(uri.Host))
        {
            errorMessage = ValidationErrorMessage;
            return false;
        }

        var asinMatch = AsinPattern().Match(uri.AbsolutePath);

        if (!asinMatch.Success)
        {
            errorMessage = ValidationErrorMessage;
            return false;
        }

        var asin = asinMatch.Groups[1].Value.ToUpperInvariant();

        if (!IsValidAsin(asin))
        {
            errorMessage = ValidationErrorMessage;
            return false;
        }

        result = new AmazonProductUrlParseResult(
            asin,
            $"https://www.amazon.com.br/dp/{asin}"
        );

        return true;
    }

    private static bool IsAllowedAmazonBrazilHost(string host)
    {
        var normalizedHost = host.ToLowerInvariant();

        if (normalizedHost is "amazon.com.br" or "www.amazon.com.br" or "m.amazon.com.br")
        {
            return true;
        }

        return normalizedHost.EndsWith(".amazon.com.br", StringComparison.Ordinal)
            && normalizedHost != "amazon.com.br";
    }

    private static bool IsValidAsin(string asin) =>
        asin.Length == 10 && AsinCharactersPattern().IsMatch(asin);

    [GeneratedRegex(@"(?:/dp/|/gp/product/)([A-Za-z0-9]{10})", RegexOptions.Compiled)]
    private static partial Regex AsinPattern();

    [GeneratedRegex("^[A-Z0-9]{10}$", RegexOptions.Compiled)]
    private static partial Regex AsinCharactersPattern();
}
