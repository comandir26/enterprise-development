using Bikes.Application.Dto;
using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Application.Services;

/// <summary>
/// A class that implements the interface of the BikeService class
/// </summary>
public class BikeService : IBikeService
{
    private readonly IRepository<Bike, int> _bikeRepository;
    private readonly IRepository<BikeModel, int> _bikeModelRepository;

    /// <summary>
    /// The constructor that initializes repositories
    /// </summary>
    /// <param name="bikeRepository"></param>
    /// <param name="bikeModelRepository"></param>
    public BikeService(
        IRepository<Bike, int> bikeRepository,
        IRepository<BikeModel, int> bikeModelRepository)
    {
        _bikeRepository = bikeRepository;
        _bikeModelRepository = bikeModelRepository;
    }

    /// <summary>
    /// A method that maps a DTO object to a domain object
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="model"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private static Bike MapToDomain(BikeDto dto, BikeModel model, int id = 0)
    {
        return new Bike
        {
            Id = id,
            SerialNumber = dto.SerialNumber,
            Color = dto.Color,
            Model = model
        };
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="bikeDto">DTO object</param>
    /// <returns>ID of the created object</returns>
    public int CreateBike(BikeDto bikeDto)
    {
        var model = _bikeModelRepository.Read(bikeDto.ModelId);
        if (model == null)
            throw new ArgumentException($"BikeModel with id {bikeDto.ModelId} not found");

        var bike = MapToDomain(bikeDto, model);
        return _bikeRepository.Create(bike);
    }

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<Bike> GetAllBikes() => _bikeRepository.ReadAll();

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Bike? GetBikeById(int id) => _bikeRepository.Read(id);

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="bikeDto">DTO object</param>
    /// <returns>Object if exist</returns>
    public Bike? UpdateBike(int id, BikeDto bikeDto)
    {
        var existingBike = _bikeRepository.Read(id);
        if (existingBike == null) return null;

        var model = _bikeModelRepository.Read(bikeDto.ModelId);
        if (model == null) return null;

        var updatedBike = MapToDomain(bikeDto, model, id);
        return _bikeRepository.Update(id, updatedBike);
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool DeleteBike(int id) => _bikeRepository.Delete(id);
}