using Bikes.Contracts.Dto;
using Bikes.Domain.Models;
using Bogus;

namespace Bikes.Generator;

/// <summary>
/// Generates fake data for various entities using Bogus library
/// </summary>
public class ContractGenerator
{
    private readonly Faker<BikeCreateUpdateDto> _bikeFaker;
    private readonly Faker<BikeModelCreateUpdateDto> _bikeModelFaker;
    private readonly Faker<RenterCreateUpdateDto> _renterFaker;
    private readonly Faker<RentCreateUpdateDto> _rentFaker;

    /// <summary>
    /// Initializes the generator with fake data rules for all entity types
    /// </summary>
    public ContractGenerator()
    {
        _bikeFaker = new Faker<BikeCreateUpdateDto>()
            .CustomInstantiator(f => new BikeCreateUpdateDto
            {
                SerialNumber = $"BIKE{f.Random.Int(1000, 9999)}",
                Color = f.PickRandom("Красный", "Синий", "Зеленый", "Черный", "Белый"),
                ModelId = f.Random.Int(1, 10)
            });

        _bikeModelFaker = new Faker<BikeModelCreateUpdateDto>()
            .CustomInstantiator(f => new BikeModelCreateUpdateDto
            {
                Type = f.PickRandom<BikeType>(),
                WheelSize = f.Random.Int(20, 29),
                MaxPassengerWeight = f.Random.Int(70, 120),
                Weight = f.Random.Int(10, 20),
                BrakeType = f.PickRandom("Дисковые гидравлические", "Ободные v-brake", "Дисковые механические"),
                Year = f.Random.Int(2020, 2024),
                RentPrice = f.Random.Int(300, 1000)
            });

        _renterFaker = new Faker<RenterCreateUpdateDto>()
            .CustomInstantiator(f => new RenterCreateUpdateDto
            {
                FullName = f.Name.FullName(),
                Number = $"+7 ({f.Random.Int(900, 999)}) {f.Random.Int(100, 999)}-{f.Random.Int(10, 99)}-{f.Random.Int(10, 99)}"
            });

        _rentFaker = new Faker<RentCreateUpdateDto>()
            .CustomInstantiator(f => new RentCreateUpdateDto
            {
                RentalStartTime = f.Date.Soon(1),
                RentalDuration = f.Random.Int(1, 24),
                RenterId = f.Random.Int(1, 10),
                BikeId = f.Random.Int(1, 10)
            });
    }

    /// <summary>
    /// Generates a fake bike DTO with random data
    /// </summary>
    /// <returns>Generated bike DTO</returns>
    public BikeCreateUpdateDto GenerateBike() => _bikeFaker.Generate();

    /// <summary>
    /// Generates a fake bike model DTO with random data
    /// </summary>
    /// <returns>Generated bike model DTO</returns>
    public BikeModelCreateUpdateDto GenerateBikeModel() => _bikeModelFaker.Generate();

    /// <summary>
    /// Generates a fake renter DTO with random data
    /// </summary>
    /// <returns>Generated renter DTO</returns>
    public RenterCreateUpdateDto GenerateRenter() => _renterFaker.Generate();

    /// <summary>
    /// Generates a fake rent DTO with random data
    /// </summary>
    /// <returns>Generated rent DTO</returns>
    public RentCreateUpdateDto GenerateRent() => _rentFaker.Generate();

    /// <summary>
    /// Generates a batch of random entities
    /// </summary>
    /// <param name="size">Number of entities to generate</param>
    /// <returns>List of generated entity DTOs</returns>
    public List<object> GenerateBatch(int size)
    {
        var batch = new List<object>();

        for (var i = 0; i < size; i++)
        {
            var entityType = new Random().Next(0, 4);
            batch.Add(entityType switch
            {
                0 => GenerateBike(),
                1 => GenerateBikeModel(),
                2 => GenerateRenter(),
                3 => GenerateRent(),
                _ => GenerateBike()
            });
        }

        return batch;
    }
}