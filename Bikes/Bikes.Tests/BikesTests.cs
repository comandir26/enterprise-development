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
        var expectedModelIds = new List<int> {2, 5, 8};

        var actualIds = fixture.Bikes
            .Where(bike => bike.Model.Type == BikeType.Sport)
            .Select(bike => bike.Id)
            .ToList();

        Assert.Equal(expectedModelIds, actualIds);
    }

    /// <summary>
    /// A test that outputs the top 5 bike models by rental duration
    /// </summary>
    [Fact]
    public void TopFiveModelsRentDurationIds()
    {
        var expectedModelIds = new List<int> {10, 1, 2, 5, 3}; 

        var actualIds = fixture.Rents
            .GroupBy(rent => rent.Bike.Model.Id)
            .Select(group => new
            {
                ModelId = group.Key,
                TotalDuration = group.Sum(rent => rent.RentalDuration)
            })
            .OrderByDescending(x => x.TotalDuration)
            .Select(x => x.ModelId)
            .Take(5)
            .ToList();

        Assert.Equal(expectedModelIds, actualIds);
    }

    /// <summary>
    /// A test that outputs the top 5 bike models in terms of rental income
    /// </summary>
    [Fact]
    public void TopFiveModelsProfit()
    {
        var expectedModelIds = new List<int> {10, 5, 2, 1, 3};

        var actualIds = fixture.Rents
            .GroupBy(rent => rent.Bike.Model.Id)
            .Select(group => new
            {
                ModelId = group.Key,
                TotalProfit = group.Sum(rent => rent.RentalDuration * rent.Bike.Model.RentPrice)
            })
            .OrderByDescending(x => x.TotalProfit)
            .Select(x => x.ModelId)
            .Take(5)
            .ToList();
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

        var durations = fixture.Rents.Select(rent => rent.RentalDuration).ToList();
        var actualMin = durations.Min();
        var actualMax = durations.Max();
        var actualAvg = durations.Average();

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
        var actualRentalTime = fixture.Rents
            .Where(rent => rent.Bike.Model.Type == bikeType)
            .Sum(rent => rent.RentalDuration);

        Assert.Equal(expectedRentalTime, actualRentalTime);
    }

    /// <summary>
    /// A test that outputs information about the customers who have rented bicycles the most times.
    /// </summary>
    [Fact]
    public void TopThreeRenters()
    {
        var expectedTopRentersIds = new List<int> {1, 2, 6};

        var actualTopRentersIds = fixture.Rents
            .GroupBy(rent => rent.Renter.Id)
            .Select(group => new
            {
                RenterId = group.Key,
                TotalRentals = group.Count()
            })
            .OrderByDescending(r => r.TotalRentals)
            .Select(x => x.RenterId)
            .Take(3)
            .ToList();

        Assert.Equal(expectedTopRentersIds, actualTopRentersIds);
    }
}