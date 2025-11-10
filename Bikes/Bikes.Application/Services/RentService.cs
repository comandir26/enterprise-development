using Bikes.Application.Dto;
using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Application.Services;

/// <summary>
/// A class that implements the interface of the RentService class
/// </summary>
public class RentService : IRentService
{
    private readonly IRepository<Rent, int> _rentRepository;
    private readonly IRepository<Bike, int> _bikeRepository;
    private readonly IRepository<Renter, int> _renterRepository;

    /// <summary>
    /// The constructor that initializes repositories
    /// </summary>
    /// <param name="rentRepository"></param>
    /// <param name="bikeRepository"></param>
    /// <param name="renterRepository"></param>
    public RentService(
        IRepository<Rent, int> rentRepository,
        IRepository<Bike, int> bikeRepository,
        IRepository<Renter, int> renterRepository)
    {
        _rentRepository = rentRepository;
        _bikeRepository = bikeRepository;
        _renterRepository = renterRepository;
    }

    /// <summary>
    /// A method that maps a DTO object to a domain object
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="bike"></param>
    /// <param name="renter"></param>
    /// <param name="id"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="rentDto"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
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

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns></returns>
    public List<Rent> GetAllRents() => _rentRepository.ReadAll();

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Rent? GetRentById(int id) => _rentRepository.Read(id);

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id"></param>
    /// <param name="rentDto"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool DeleteRent(int id) => _rentRepository.Delete(id);
}