using Bikes.Application.Dto;
using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Application.Services;

/// <summary>
/// A class that implements the interface of the BikeModelService class
/// </summary>
public class BikeModelService : IBikeModelService
{
    private readonly IRepository<BikeModel, int> _bikeModelRepository;

    /// <summary>
    /// The constructor that initializes repositories
    /// </summary>
    /// <param name="bikeModelRepository"></param>
    public BikeModelService(IRepository<BikeModel, int> bikeModelRepository)
    {
        _bikeModelRepository = bikeModelRepository;
    }

    /// <summary>
    /// A method that maps a DTO object to a domain object
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private static BikeModel MapToDomain(BikeModelDto dto, int id = 0)
    {
        return new BikeModel
        {
            Id = id,
            Type = dto.Type,
            WheelSize = dto.WheelSize,
            MaxPassengerWeight = dto.MaxPassengerWeight,
            Weight = dto.Weight,
            BrakeType = dto.BrakeType,
            Year = dto.Year,
            RentPrice = dto.RentPrice
        };
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="bikeModelDto">DTO object</param>
    /// <returns>ID of the created object</returns>
    public int CreateBikeModel(BikeModelDto bikeModelDto)
    {
        var bikeModel = MapToDomain(bikeModelDto);
        return _bikeModelRepository.Create(bikeModel);
    }

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<BikeModel> GetAllBikeModels() => _bikeModelRepository.ReadAll();

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BikeModel? GetBikeModelById(int id) => _bikeModelRepository.Read(id);

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="bikeModelDto">DTO object</param>
    /// <returns>Object if exist</returns>
    public BikeModel? UpdateBikeModel(int id, BikeModelDto bikeModelDto)
    {
        var existingModel = _bikeModelRepository.Read(id);
        if (existingModel == null) return null;

        var updatedModel = MapToDomain(bikeModelDto, id);
        return _bikeModelRepository.Update(id, updatedModel);
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool DeleteBikeModel(int id) => _bikeModelRepository.Delete(id);
}