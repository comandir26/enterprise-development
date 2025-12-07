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
        var expectedSerialNumbers = new List<string>
        {
            "SPT202402001",  // Bike Id 2
            "SPT202403001",  // Bike Id 5  
            "SPT202305001"   // Bike Id 8
        };

        var sportBikes = fixture.AnalyticsService.GetSportBikes();

        Assert.Equal(3, sportBikes.Count);

        var actualSerialNumbers = sportBikes.Select(bike => bike.SerialNumber).ToList();
        Assert.Equal(expectedSerialNumbers, actualSerialNumbers);
    }

    /// <summary>
    /// A test that outputs the top 5 bike models by rental duration
    /// </summary>
    [Fact]
    public void TopFiveModelsRentDurationIds()
    {
        var expectedModelTypes = new List<BikeType>
        {
            BikeType.Mountain, // Model Id 10
            BikeType.Mountain, // Model Id 1  
            BikeType.Sport,    // Model Id 2
            BikeType.Sport,    // Model Id 5
            BikeType.City      // Model Id 3
        };

        var topModels = fixture.AnalyticsService.GetTopFiveModelsByRentDuration();

        Assert.Equal(5, topModels.Count);

        var actualModelTypes = topModels.Select(model => model.Type).ToList();
        Assert.Equal(expectedModelTypes, actualModelTypes);
    }

    /// <summary>
    /// A test that outputs the top 5 bike models in terms of rental income
    /// </summary>
    [Fact]
    public void TopFiveModelsProfit()
    {
        var expectedModelTypes = new List<BikeType>
        {
            BikeType.Mountain, // Model Id 10
            BikeType.Sport,    // Model Id 5
            BikeType.Sport,    // Model Id 2
            BikeType.Mountain, // Model Id 1
            BikeType.City      // Model Id 3
        };

        var topModels = fixture.AnalyticsService.GetTopFiveModelsByProfit();

        Assert.Equal(5, topModels.Count);

        var actualModelTypes = topModels.Select(model => model.Type).ToList();
        Assert.Equal(expectedModelTypes, actualModelTypes);
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

        var stats = fixture.AnalyticsService.GetRentalDurationStats();

        Assert.Equal(expectedMin, stats.Min);
        Assert.Equal(expectedMax, stats.Max);
        Assert.Equal(expectedAvg, stats.Average);
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
        var expectedFullNames = new List<string>
        {
            "Иванов Иван Иванович",    // Renter Id 1
            "Петров Петр Сергеевич",   // Renter Id 2
            "Попов Денис Андреевич"    // Renter Id 6
        };

        var topRenters = fixture.AnalyticsService.GetTopThreeRenters();

        Assert.Equal(3, topRenters.Count);

        var actualFullNames = topRenters.Select(renter => renter.FullName).ToList();
        Assert.Equal(expectedFullNames, actualFullNames);
    }
}