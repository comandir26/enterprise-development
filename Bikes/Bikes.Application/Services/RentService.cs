using AutoMapper;
using Bikes.Contracts.Dto;
using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Application.Services;

/// <summary>
/// A class that implements the interface of the RentService class
/// </summary>
public class RentService(
    IRepository<Rent, int> rentRepository,
    IRepository<Bike, int> bikeRepository,
    IRepository<Renter, int> renterRepository,
    IMapper mapper) : IRentService
{
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="rentDto"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public int CreateRent(RentDto rentDto)
    {
        var bike = bikeRepository.Read(rentDto.BikeId)
            ?? throw new ArgumentException($"Bike with id {rentDto.BikeId} not found");

        var renter = renterRepository.Read(rentDto.RenterId)
            ?? throw new ArgumentException($"Renter with id {rentDto.RenterId} not found");

        var rent = mapper.Map<Rent>(rentDto);
        rent.Bike = bike;
        rent.Renter = renter;

        return rentRepository.Create(rent);
    }

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns></returns>
    public List<RentDto> GetAllRents()
    {
        var rents = rentRepository.ReadAll();
        return mapper.Map<List<RentDto>>(rents);
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public RentDto? GetRentById(int id)
    {
        var rent = rentRepository.Read(id);
        return rent != null ? mapper.Map<RentDto>(rent) : null;
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id"></param>
    /// <param name="rentDto"></param>
    /// <returns></returns>
    public RentDto? UpdateRent(int id, RentDto rentDto)
    {
        var existingRent = rentRepository.Read(id);
        if (existingRent == null) return null;

        var bike = bikeRepository.Read(rentDto.BikeId);
        if (bike == null) return null;

        var renter = renterRepository.Read(rentDto.RenterId);
        if (renter == null) return null;

        mapper.Map(rentDto, existingRent);

        existingRent.Bike = bike;
        existingRent.Renter = renter;

        var updatedRent = rentRepository.Update(id, existingRent);
        return updatedRent != null ? mapper.Map<RentDto>(updatedRent) : null;
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool DeleteRent(int id) => rentRepository.Delete(id);
}