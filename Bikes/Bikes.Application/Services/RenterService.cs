using Bikes.Application.Dto;
using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Application.Services;

/// <summary>
/// A class that implements the interface of the RenterService class
/// </summary>
public class RenterService : IRenterService
{
    private readonly IRepository<Renter, int> _renterRepository;

    /// <summary>
    /// The constructor that initializes repositories
    /// </summary>
    /// <param name="renterRepository"></param>
    public RenterService(IRepository<Renter, int> renterRepository)
    {
        _renterRepository = renterRepository;
    }

    /// <summary>
    /// A method that maps a DTO object to a domain object
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private static Renter MapToDomain(RenterDto dto, int id = 0)
    {
        return new Renter
        {
            Id = id,
            FullName = dto.FullName,
            Number = dto.Number
        };
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="renterDto"></param>
    /// <returns></returns>
    public int CreateRenter(RenterDto renterDto)
    {
        var renter = MapToDomain(renterDto);
        return _renterRepository.Create(renter);
    }

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns></returns>
    public List<Renter> GetAllRenters() => _renterRepository.ReadAll();

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Renter? GetRenterById(int id) => _renterRepository.Read(id);

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id"></param>
    /// <param name="renterDto"></param>
    /// <returns></returns>
    public Renter? UpdateRenter(int id, RenterDto renterDto)
    {
        var existingRenter = _renterRepository.Read(id);
        if (existingRenter == null) return null;

        var updatedRenter = MapToDomain(renterDto, id);
        return _renterRepository.Update(id, updatedRenter);
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool DeleteRenter(int id) => _renterRepository.Delete(id);
}
