using AutoMapper;
using Bikes.Application.Interfaces;
using Bikes.Contracts.Dto;
using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Application.Services;

/// <summary>
/// A class that implements the interface of the BikeService class
/// </summary>
public class BikeService(
    IRepository<Bike, int> bikeRepository,
    IRepository<BikeModel, int> bikeModelRepository,
    IMapper mapper) : IBikeService
{
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="bikeDto">DTO object</param>
    /// <returns>ID of the created object</returns>
    public int CreateBike(BikeCreateUpdateDto bikeDto)
    {
        var model = bikeModelRepository.Read(bikeDto.ModelId)
            ?? throw new ArgumentException($"BikeModel with id {bikeDto.ModelId} not found");

        var bike = mapper.Map<Bike>(bikeDto);
        bike.Model = model;

        return bikeRepository.Create(bike);
    }

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<BikeGetDto> GetAllBikes()
    {
        var bikes = bikeRepository.ReadAll();
        return mapper.Map<List<BikeGetDto>>(bikes);
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BikeGetDto? GetBikeById(int id)
    {
        var bike = bikeRepository.Read(id);
        return bike != null ? mapper.Map<BikeGetDto>(bike) : null;
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="bikeDto">DTO object</param>
    /// <returns>Object if exist</returns>
    public BikeGetDto? UpdateBike(int id, BikeCreateUpdateDto bikeDto)
    {
        var existingBike = bikeRepository.Read(id);
        if (existingBike == null) return null;

        var model = bikeModelRepository.Read(bikeDto.ModelId);
        if (model == null) return null;

        mapper.Map(bikeDto, existingBike);

        existingBike.Model = model;

        var updatedBike = bikeRepository.Update(id, existingBike);
        return updatedBike != null ? mapper.Map<BikeGetDto>(updatedBike) : null;
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool DeleteBike(int id) => bikeRepository.Delete(id);
}