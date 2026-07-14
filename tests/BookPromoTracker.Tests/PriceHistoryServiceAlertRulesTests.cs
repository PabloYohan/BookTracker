using BookPromoTracker.Services;

namespace BookPromoTracker.Tests;

public class PriceHistoryServiceAlertRulesTests
{
    [Fact]
    public void ShouldCreateAlert_FirstPriceBelowTarget_ReturnsTrue()
    {
        var shouldCreate = PriceHistoryService.ShouldCreateAlert(
            newPrice: 29.90m,
            targetPrice: 35m,
            previousPrice: null,
            mostRecentAlertPrice: null
        );

        Assert.True(shouldCreate);
    }

    [Fact]
    public void ShouldCreateAlert_CrossingTarget_ReturnsTrue()
    {
        var shouldCreate = PriceHistoryService.ShouldCreateAlert(
            newPrice: 39.90m,
            targetPrice: 40m,
            previousPrice: 45m,
            mostRecentAlertPrice: null
        );

        Assert.True(shouldCreate);
    }

    [Fact]
    public void ShouldCreateAlert_RepeatedPrice_ReturnsFalse()
    {
        var shouldCreate = PriceHistoryService.ShouldCreateAlert(
            newPrice: 29.90m,
            targetPrice: 35m,
            previousPrice: 29.90m,
            mostRecentAlertPrice: 29.90m
        );

        Assert.False(shouldCreate);
    }

    [Fact]
    public void ShouldCreateAlert_NewLowerPriceBelowTarget_ReturnsTrue()
    {
        var shouldCreate = PriceHistoryService.ShouldCreateAlert(
            newPrice: 34.90m,
            targetPrice: 40m,
            previousPrice: 39.90m,
            mostRecentAlertPrice: 39.90m
        );

        Assert.True(shouldCreate);
    }
}
