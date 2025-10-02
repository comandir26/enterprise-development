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
                Model = fixture.BikeModels.First(m => m.Id == group.Key),
                TotalDuration = group.Sum(rent => rent.RentalDuration)
            })
            .OrderByDescending(x => x.TotalDuration)
            .Select(x => x.Model.Id)
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
                Model = fixture.BikeModels.First(m => m.Id == group.Key),
                TotalProfit = group.Sum(rent => rent.RentalDuration * rent.Bike.Model.RentPrice)
            })
            .OrderByDescending(x => x.TotalProfit)
            .Select(x => x.Model.Id)
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

        var actualMin = fixture.Rents.Min(rent => rent.RentalDuration);
        var actualMax = fixture.Rents.Max(rent => rent.RentalDuration);
        var actualAvg = fixture.Rents.Average(rent => rent.RentalDuration);

        Assert.Equal(expectedMin, actualMin);
        Assert.Equal(expectedMax, actualMax);
        Assert.Equal(expectedAvg, actualAvg);
    }

    /// <summary>
    /// A test that outputs the total rental time of each type of bike
    /// </summary>
    [Fact]
    public void TotalRentalTimeByType() 
    {
        var expectedSportRentalTime = 17;
        var expectedMountainRentaltime = 30;
        var expectedCityRentaltime = 12;

        var actual = fixture.Rents
            .GroupBy(rent => rent.Bike.Model.Type)
            .Select(group => new
            {
                ModelType = group.Key,
                TotalRentalTime = group.Sum(rent => rent.RentalDuration)
            })
            .Select(x => x)
            .ToList();

        var actualSportRentalTime = actual.Where(x => x.ModelType == BikeType.Sport).Select(x => x.TotalRentalTime).First();
        var actualMountainRentaltime = actual.Where(x => x.ModelType == BikeType.Mountain).Select(x => x.TotalRentalTime).First();
        var actualCityRentaltime = actual.Where(x => x.ModelType == BikeType.City).Select(x => x.TotalRentalTime).First();

        Assert.Equal(expectedSportRentalTime, actualSportRentalTime);
        Assert.Equal(expectedMountainRentaltime, actualMountainRentaltime);
        Assert.Equal(expectedCityRentaltime, actualCityRentaltime);
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
                RenterName = fixture.Renters.First(r => r.Id == group.Key),
                TotalRentals = group.Count()
            })
            .OrderByDescending(r => r.TotalRentals)
            .Select(x => x.RenterId)
            .Take(3)
            .ToList();

        Assert.Equal(expectedTopRentersIds, actualTopRentersIds);
    }
}