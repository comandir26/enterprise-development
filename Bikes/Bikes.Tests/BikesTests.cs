using Bikes.Domain.Models;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Xunit.Sdk;

namespace Bikes.Tests;

public class BikesTests(BikesFixture fixture) : IClassFixture<BikesFixture>
{
    [Fact]
    public void InformationAboutSportBikes()
    {
        var expected = new List<Bike>
        {
            new() { Id = 2, SerialNumber = "SPT202402001", Color = "Красный", Model = fixture.BikeModels[1] },
            new() { Id = 5, SerialNumber = "SPT202403001", Color = "Желтый", Model = fixture.BikeModels[4] },
            new() { Id = 8, SerialNumber = "SPT202305001", Color = "Фиолетовый", Model = fixture.BikeModels[7] },
        };

        var actual = fixture.Bikes
            .Where(bike => bike.Model.Type == BikeType.Sport)
            .Select(bike => bike.Id)
            .ToList();


        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i].Id, actual[i]);
        }
    }

    [Fact]
    public void TopFiveModelsRentDuration()
    {
        var expected = new List<BikeModel>
        {
            new() { Id = 10, Type = BikeType.Mountain, WheelSize = 26, MaxPassengerWeight = 130, Weight = 14, BrakeType = "Дисковые гидравлические", Year = "2024", RentPrice = 650 },
            new() { Id = 1, Type = BikeType.Mountain, WheelSize = 29, MaxPassengerWeight = 120, Weight = 14, BrakeType = "Дисковые гидравлические", Year = "2023", RentPrice = 700 },
            new() { Id = 2, Type = BikeType.Sport, WheelSize = 27, MaxPassengerWeight = 110, Weight = 11, BrakeType = "Ободные v-brake", Year = "2024", RentPrice = 850 },
            new() { Id = 5, Type = BikeType.Sport, WheelSize = 26, MaxPassengerWeight = 115, Weight = 12, BrakeType = "Ободные карбоновые", Year = "2024", RentPrice = 900 },
            new() { Id = 3, Type = BikeType.City, WheelSize = 26, MaxPassengerWeight = 130, Weight = 16, BrakeType = "Дисковые механические", Year = "2022", RentPrice = 500 }
        };

        var actual = fixture.Rents
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

        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i].Id, actual[i]);
        }
    }

    [Fact]
    public void TopFiveModelsProfit()
    {
        var expected = new List<BikeModel>
        {
            new() { Id = 10, Type = BikeType.Mountain, WheelSize = 26, MaxPassengerWeight = 130, Weight = 14, BrakeType = "Дисковые гидравлические", Year = "2024", RentPrice = 650 },
            new() { Id = 5, Type = BikeType.Sport, WheelSize = 26, MaxPassengerWeight = 115, Weight = 12, BrakeType = "Ободные карбоновые", Year = "2024", RentPrice = 900 },
            new() { Id = 2, Type = BikeType.Sport, WheelSize = 27, MaxPassengerWeight = 110, Weight = 11, BrakeType = "Ободные v-brake", Year = "2024", RentPrice = 850 },
            new() { Id = 1, Type = BikeType.Mountain, WheelSize = 29, MaxPassengerWeight = 120, Weight = 14, BrakeType = "Дисковые гидравлические", Year = "2023", RentPrice = 700 },
            new() { Id = 3, Type = BikeType.City, WheelSize = 26, MaxPassengerWeight = 130, Weight = 16, BrakeType = "Дисковые механические", Year = "2022", RentPrice = 500 }
        };

        var actual = fixture.Rents
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

        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i].Id, actual[i]);
        }
    }

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

    [Fact]
    public void TopThreeRenters()
    {
        var expectedTopRenters = new List<Renter>
        {
            new() { Id = 1, FullName = "Иванов Иван Иванович", Number = "+7 (912) 345-67-89" },
            new() { Id = 2, FullName = "Петров Петр Сергеевич", Number = "+7 (923) 456-78-90" },
            new() { Id = 6, FullName = "Попов Денис Андреевич", Number = "+7 (967) 890-12-34" },
        };

        var actualTopRenters = fixture.Rents
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

        for (var i = 0; i < actualTopRenters.Count; i++)
        {
            Assert.Equal(expectedTopRenters[i].Id, actualTopRenters[i]);
        }
    }
}
