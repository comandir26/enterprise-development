using Bikes.Contracts.Dto;

namespace Bikes.Application.Interfaces;

/// <summary>
/// Interface for the BikeModel service class
/// </summary>
public interface IBikeModelService
{
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="bikeModelDto">DTO object</param>
    /// <returns>Created object</returns>
    public BikeModelGetDto CreateBikeModel(BikeModelCreateUpdateDto bikeModelDto);

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<BikeModelGetDto> GetAllBikeModels();

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BikeModelGetDto? GetBikeModelById(int id);

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="bikeModelDto">DTO object</param>
    /// <returns>Object if exist</returns>
    public BikeModelGetDto? UpdateBikeModel(int id, BikeModelCreateUpdateDto bikeModelDto);

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool DeleteBikeModel(int id);
}