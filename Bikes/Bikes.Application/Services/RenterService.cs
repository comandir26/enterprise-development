using AutoMapper;
using Bikes.Application.Interfaces;
using Bikes.Contracts.Dto;
using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Application.Services;

/// <summary>
/// A class that implements the interface of the RenterService class
/// </summary>
public class RenterService(
    IRepository<Renter, int> renterRepository,
    IMapper mapper) : IRenterService
{
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="renterDto"></param>
    /// <returns></returns>
    public RenterGetDto CreateRenter(RenterCreateUpdateDto renterDto)
    {
        var renter = mapper.Map<Renter>(renterDto);

        var id = renterRepository.Create(renter);
        var createdRenter = renterRepository.Read(id);

        return mapper.Map<RenterGetDto>(createdRenter);
    }

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns></returns>
    public List<RenterGetDto> GetAllRenters()
    {
        var renters = renterRepository.ReadAll();
        return mapper.Map<List<RenterGetDto>>(renters);
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public RenterGetDto? GetRenterById(int id)
    {
        var renter = renterRepository.Read(id);
        return renter != null ? mapper.Map<RenterGetDto>(renter) : null;
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id"></param>
    /// <param name="renterDto"></param>
    /// <returns></returns>
    public RenterGetDto? UpdateRenter(int id, RenterCreateUpdateDto renterDto)
    {
        var existingRenter = renterRepository.Read(id);
        if (existingRenter == null) return null;

        mapper.Map(renterDto, existingRenter);

        var updatedRenter = renterRepository.Update(id, existingRenter);
        return updatedRenter != null ? mapper.Map<RenterGetDto>(updatedRenter) : null;
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool DeleteRenter(int id) => renterRepository.Delete(id);
}