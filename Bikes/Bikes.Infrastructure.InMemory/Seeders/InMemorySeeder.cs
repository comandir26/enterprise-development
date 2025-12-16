using Bikes.Domain.Models;

namespace Bikes.Infrastructure.InMemory.Seeders;

/// <summary>
/// InMemorySeeder for creating the data
/// </summary>
public static class InMemorySeeder
{
    /// <summary>
    /// A static method that creates a list of bike models
    /// </summary>
    public static List<BikeModel> GetBikeModels()
    {
        return
        [
            new() { Id = 1, Type = BikeType.Mountain, WheelSize = 29, MaxPassengerWeight = 120, Weight = 14, BrakeType = "Дисковые гидравлические", Year = 2023, RentPrice = 700 },
            new() { Id = 2, Type = BikeType.Sport, WheelSize = 27, MaxPassengerWeight = 110, Weight = 11, BrakeType = "Ободные v-brake", Year = 2024, RentPrice = 850 },
            new() { Id = 3, Type = BikeType.City, WheelSize = 26, MaxPassengerWeight = 130, Weight = 16, BrakeType = "Дисковые механические", Year = 2022, RentPrice = 500 },
            new() { Id = 4, Type = BikeType.Mountain, WheelSize = 26, MaxPassengerWeight = 125, Weight = 15, BrakeType = "Дисковые гидравлические", Year = 2023, RentPrice = 750 },
            new() { Id = 5, Type = BikeType.Sport, WheelSize = 26, MaxPassengerWeight = 115, Weight = 12, BrakeType = "Ободные карбоновые", Year = 2024, RentPrice = 900 },
            new() { Id = 6, Type = BikeType.City, WheelSize = 27, MaxPassengerWeight = 135, Weight = 17, BrakeType = "Дисковые механические", Year = 2023, RentPrice = 550 },
            new() { Id = 7, Type = BikeType.Mountain, WheelSize = 29, MaxPassengerWeight = 120, Weight = 13, BrakeType = "Дисковые гидравлические", Year = 2024, RentPrice = 800 },
            new() { Id = 8, Type = BikeType.Sport, WheelSize = 27, MaxPassengerWeight = 110, Weight = 10, BrakeType = "Ободные v-brake", Year = 2023, RentPrice = 950 },
            new() { Id = 9, Type = BikeType.City, WheelSize = 26, MaxPassengerWeight = 140, Weight = 18, BrakeType = "Дисковые механические", Year = 2022, RentPrice = 600 },
            new() { Id = 10, Type = BikeType.Mountain, WheelSize = 26, MaxPassengerWeight = 130, Weight = 14, BrakeType = "Дисковые гидравлические", Year = 2024, RentPrice = 650 }
        ];
    }

    /// <summary>
    /// A static method that creates a list of bikes
    /// </summary>
    public static List<Bike> GetBikes()
    {
        var models = GetBikeModels();

        return
        [
            new() { Id = 1, SerialNumber = "MTB202301001", Color = "Черный", ModelId = models[0].Id, Model = models[0] },
            new() { Id = 2, SerialNumber = "SPT202402001", Color = "Красный", ModelId = models[1].Id, Model = models[1] },
            new() { Id = 3, SerialNumber = "CTY202203001", Color = "Синий", ModelId = models[2].Id, Model = models[2] },
            new() { Id = 4, SerialNumber = "MTB202302001", Color = "Зеленый", ModelId = models[3].Id, Model = models[3] },
            new() { Id = 5, SerialNumber = "SPT202403001", Color = "Желтый", ModelId = models[4].Id, Model = models[4] },
            new() { Id = 6, SerialNumber = "CTY202304001", Color = "Белый", ModelId = models[5].Id, Model = models[5] },
            new() { Id = 7, SerialNumber = "MTB202404001", Color = "Оранжевый", ModelId = models[6].Id, Model = models[6] },
            new() { Id = 8, SerialNumber = "SPT202305001", Color = "Фиолетовый", ModelId = models[7].Id, Model = models[7] },
            new() { Id = 9, SerialNumber = "CTY202205001", Color = "Серый", ModelId = models[8].Id, Model = models[8] },
            new() { Id = 10, SerialNumber = "MTB202405001", Color = "Голубой", ModelId = models[9].Id, Model = models[9] }
        ];
    }

    /// <summary>
    /// A static method that creates a list of renters
    /// </summary>
    public static List<Renter> GetRenters()
    {
        return
        [
            new() { Id = 1, FullName = "Иванов Иван Иванович", Number = "+7 (912) 345-67-89" },
            new() { Id = 2, FullName = "Петров Петр Сергеевич", Number = "+7 (923) 456-78-90" },
            new() { Id = 3, FullName = "Сидорова Анна Владимировна", Number = "+7 (934) 567-89-01" },
            new() { Id = 4, FullName = "Кузнецов Алексей Дмитриевич", Number = "+7 (945) 678-90-12" },
            new() { Id = 5, FullName = "Смирнова Екатерина Олеговна", Number = "+7 (956) 789-01-23" },
            new() { Id = 6, FullName = "Попов Денис Андреевич", Number = "+7 (967) 890-12-34" },
            new() { Id = 7, FullName = "Васильева Мария Игоревна", Number = "+7 (978) 901-23-45" },
            new() { Id = 8, FullName = "Николаев Сергей Викторович", Number = "+7 (989) 012-34-56" },
            new() { Id = 9, FullName = "Орлова Ольга Павловна", Number = "+7 (990) 123-45-67" },
            new() { Id = 10, FullName = "Федоров Артем Константинович", Number = "+7 (901) 234-56-78" }
        ];
    }

    /// <summary>
    /// A static method that creates a list of rents
    /// </summary>
    public static List<Rent> GetRents()
    {
        var bikes = GetBikes();
        var renters = GetRenters();

        return
        [
            new() { Id = 1, RentalStartTime = new DateTime(2025, 6, 10, 9, 0, 0), RentalDuration = 3, RenterId = renters[0].Id, BikeId = bikes[0].Id, Renter = renters[0], Bike = bikes[0] },
            new() { Id = 2, RentalStartTime = new DateTime(2025, 6, 12, 14, 30, 0), RentalDuration = 2, RenterId = renters[1].Id, BikeId = bikes[0].Id, Renter = renters[1], Bike = bikes[0] },
            new() { Id = 3, RentalStartTime = new DateTime(2025, 6, 15, 10, 0, 0), RentalDuration = 4, RenterId = renters[2].Id, BikeId = bikes[0].Id, Renter = renters[2], Bike = bikes[0] },
            new() { Id = 4, RentalStartTime = new DateTime(2025, 6, 18, 16, 0, 0), RentalDuration = 1, RenterId = renters[3].Id, BikeId = bikes[1].Id, Renter = renters[3], Bike = bikes[1] },
            new() { Id = 5, RentalStartTime = new DateTime(2025, 6, 20, 11, 0, 0), RentalDuration = 5, RenterId = renters[4].Id, BikeId = bikes[1].Id, Renter = renters[4], Bike = bikes[1] },
            new() { Id = 6, RentalStartTime = new DateTime(2025, 6, 22, 13, 0, 0), RentalDuration = 2, RenterId = renters[5].Id, BikeId = bikes[1].Id, Renter = renters[5], Bike = bikes[1] },
            new() { Id = 7, RentalStartTime = new DateTime(2025, 6, 25, 15, 30, 0), RentalDuration = 3, RenterId = renters[6].Id, BikeId = bikes[2].Id, Renter = renters[6], Bike = bikes[2] },
            new() { Id = 8, RentalStartTime = new DateTime(2025, 6, 28, 9, 30, 0), RentalDuration = 4, RenterId = renters[7].Id, BikeId = bikes[2].Id, Renter = renters[7], Bike = bikes[2] },
            new() { Id = 9, RentalStartTime = new DateTime(2025, 7, 1, 12, 0, 0), RentalDuration = 1, RenterId = renters[8].Id, BikeId = bikes[3].Id, Renter = renters[8], Bike = bikes[3] },
            new() { Id = 10, RentalStartTime = new DateTime(2025, 7, 3, 17, 0, 0), RentalDuration = 2, RenterId = renters[9].Id, BikeId = bikes[3].Id, Renter = renters[9], Bike = bikes[3] },
            new() { Id = 11, RentalStartTime = new DateTime(2025, 7, 5, 10, 0, 0), RentalDuration = 3, RenterId = renters[0].Id, BikeId = bikes[4].Id, Renter = renters[0], Bike = bikes[4] },
            new() { Id = 12, RentalStartTime = new DateTime(2025, 7, 8, 14, 0, 0), RentalDuration = 5, RenterId = renters[0].Id, BikeId = bikes[4].Id, Renter = renters[0], Bike = bikes[4] },
            new() { Id = 13, RentalStartTime = new DateTime(2025, 7, 10, 16, 30, 0), RentalDuration = 2, RenterId = renters[0].Id, BikeId = bikes[5].Id, Renter = renters[0], Bike = bikes[5] },
            new() { Id = 14, RentalStartTime = new DateTime(2025, 7, 12, 11, 0, 0), RentalDuration = 4, RenterId = renters[0].Id, BikeId = bikes[6].Id, Renter = renters[0], Bike = bikes[6] },
            new() { Id = 15, RentalStartTime = new DateTime(2025, 7, 15, 13, 0, 0), RentalDuration = 1, RenterId = renters[1].Id, BikeId = bikes[7].Id, Renter = renters[1], Bike = bikes[7] },
            new() { Id = 16, RentalStartTime = new DateTime(2025, 7, 18, 15, 0, 0), RentalDuration = 3, RenterId = renters[1].Id, BikeId = bikes[8].Id, Renter = renters[1], Bike = bikes[8] },
            new() { Id = 17, RentalStartTime = new DateTime(2025, 7, 20, 9, 0, 0), RentalDuration = 2, RenterId = renters[1].Id, BikeId = bikes[9].Id, Renter = renters[1], Bike = bikes[9] },
            new() { Id = 18, RentalStartTime = new DateTime(2025, 7, 22, 12, 30, 0), RentalDuration = 5, RenterId = renters[5].Id, BikeId = bikes[9].Id, Renter = renters[5], Bike = bikes[9] },
            new() { Id = 19, RentalStartTime = new DateTime(2025, 7, 25, 14, 0, 0), RentalDuration = 3, RenterId = renters[5].Id, BikeId = bikes[9].Id, Renter = renters[5], Bike = bikes[9] },
            new() { Id = 20, RentalStartTime = new DateTime(2025, 7, 28, 16, 0, 0), RentalDuration = 4, RenterId = renters[2].Id, BikeId = bikes[9].Id, Renter = renters[2], Bike = bikes[9] }
        ];
    }

}

