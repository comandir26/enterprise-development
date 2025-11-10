using Bikes.Application.Dto;
using Bikes.Domain.Models;

namespace Bikes.Application.Services;

/// <summary>
/// Interface for the BikeModel service class
/// </summary>
public interface IBikeModelService
{
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="bikeModelDto">DTO object</param>
    /// <returns>ID of the created object</returns>
    public int CreateBikeModel(BikeModelDto bikeModelDto);

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<BikeModel> GetAllBikeModels();

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BikeModel? GetBikeModelById(int id);

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="bikeModelDto">DTO object</param>
    /// <returns>Object if exist</returns>
    public BikeModel? UpdateBikeModel(int id, BikeModelDto bikeModelDto);

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool DeleteBikeModel(int id);
}