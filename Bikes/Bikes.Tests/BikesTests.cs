using Bikes.Domain.Models;

namespace Bikes.Tests;

/// <summary>
/// A class that implements a set of unit tests
/// </summary>
public class BikesTests(BikesFixture fixture) : IClassFixture<BikesFixture>
{
    /// <summary>
    /// A test that outputs information about all sports bikes
    /// </summary>
    [Fact]
    public void InformationAboutSportBikes()
    {
        var expectedBikeIds = new List<int> { 2, 5, 8 };

        var sportBikes = fixture.AnalyticsService.GetSportBikes();
        var actualIds = sportBikes.Select(bike => bike.Id).ToList();

        Assert.Equal(expectedBikeIds, actualIds);
    }

    /// <summary>
    /// A test that outputs the top 5 bike models by rental duration
    /// </summary>
    [Fact]
    public void TopFiveModelsRentDurationIds()
    {
        var expectedModelIds = new List<int> { 10, 1, 2, 5, 3 };

        var topModels = fixture.AnalyticsService.GetTopFiveModelsByRentDuration();
        var actualIds = topModels.Select(model => model.Id).ToList();

        Assert.Equal(expectedModelIds, actualIds);
    }

    /// <summary>
    /// A test that outputs the top 5 bike models in terms of rental income
    /// </summary>
    [Fact]
    public void TopFiveModelsProfit()
    {
        var expectedModelIds = new List<int> { 10, 5, 2, 1, 3 };

        var topModels = fixture.AnalyticsService.GetTopFiveModelsByProfit();
        var actualIds = topModels.Select(model => model.Id).ToList();

        Assert.Equal(expectedModelIds, actualIds);
    }

    /// <summary>
    /// A test that outputs information about the minimum, maximum, and average bike rental time.
    /// </summary>
    [Fact]
    public void MinMaxAvgRentalDuration()
    {
        const int expectedMin = 1;
        const int expectedMax = 5;
        const double expectedAvg = 2.95;

        var (actualMin, actualMax, actualAvg) = fixture.AnalyticsService.GetRentalDurationStats();

        Assert.Equal(expectedMin, actualMin);
        Assert.Equal(expectedMax, actualMax);
        Assert.Equal(expectedAvg, actualAvg);
    }

    /// <summary>
    /// A test that outputs the total rental time of each type of bike
    /// </summary>
    [Theory]
    [InlineData(BikeType.Sport, 17)]
    [InlineData(BikeType.Mountain, 30)]
    [InlineData(BikeType.City, 12)]
    public void TotalRentalTimeByType(BikeType bikeType, int expectedRentalTime)
    {
        var rentalTimeByType = fixture.AnalyticsService.GetTotalRentalTimeByType();
        var actualRentalTime = rentalTimeByType[bikeType];

        Assert.Equal(expectedRentalTime, actualRentalTime);
    }

    /// <summary>
    /// A test that outputs information about the customers who have rented bicycles the most times.
    /// </summary>
    [Fact]
    public void TopThreeRenters()
    {
        var expectedTopRentersIds = new List<int> { 1, 2, 6 };

        var topRenters = fixture.AnalyticsService.GetTopThreeRenters();
        var actualTopRentersIds = topRenters.Select(renter => renter.Id).ToList();

        Assert.Equal(expectedTopRentersIds, actualTopRentersIds);
    }
}