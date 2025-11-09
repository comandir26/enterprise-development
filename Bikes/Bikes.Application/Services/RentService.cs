using Bikes.Application.Dto;
using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Application.Services;

public class RentService : IRentService
{
    private readonly IRepository<Rent, int> _rentRepository;
    private readonly IRepository<Bike, int> _bikeRepository;
    private readonly IRepository<Renter, int> _renterRepository;

    public RentService(
        IRepository<Rent, int> rentRepository,
        IRepository<Bike, int> bikeRepository,
        IRepository<Renter, int> renterRepository)
    {
        _rentRepository = rentRepository;
        _bikeRepository = bikeRepository;
        _renterRepository = renterRepository;
    }

 
    private static Rent MapToDomain(RentDto dto, Bike bike, Renter renter, int id = 0)
    {
        return new Rent
        {
            Id = id,
            RentalStartTime = dto.RentalStartTime,
            RentalDuration = dto.RentalDuration,
            Bike = bike,       
            Renter = renter    
        };
    }

    public int CreateRent(RentDto rentDto)
    {
        var bike = _bikeRepository.Read(rentDto.BikeId);
        if (bike == null)
            throw new ArgumentException($"Bike with id {rentDto.BikeId} not found");

        var renter = _renterRepository.Read(rentDto.RenterId);
        if (renter == null)
            throw new ArgumentException($"Renter with id {rentDto.RenterId} not found");

        var rent = MapToDomain(rentDto, bike, renter);
        return _rentRepository.Create(rent);
    }

    public List<Rent> GetAllRents() => _rentRepository.ReadAll();

    public Rent? GetRentById(int id) => _rentRepository.Read(id);

    public Rent? UpdateRent(int id, RentDto rentDto)
    {
        var existingRent = _rentRepository.Read(id);
        if (existingRent == null) return null;

        var bike = _bikeRepository.Read(rentDto.BikeId);
        if (bike == null) return null;

        var renter = _renterRepository.Read(rentDto.RenterId);
        if (renter == null) return null;

        var updatedRent = MapToDomain(rentDto, bike, renter, id);
        return _rentRepository.Update(id, updatedRent);
    }

    public bool DeleteRent(int id) => _rentRepository.Delete(id);
}