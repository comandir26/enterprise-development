using Bikes.Contracts.Dto;

namespace Bikes.Application.Interfaces;

/// <summary>
/// Interface for the Bike service class
/// </summary>
public interface IBikeService
{
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="bikeDto">DTO object</param>
    /// <returns>ID of the created object</returns>
    public BikeGetDto CreateBike(BikeCreateUpdateDto bikeDto);

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<BikeGetDto> GetAllBikes();

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BikeGetDto? GetBikeById(int id);

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="bikeDto">DTO object</param>
    /// <returns>Object if exist</returns>
    public BikeGetDto? UpdateBike(int id, BikeCreateUpdateDto bikeDto);

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool DeleteBike(int id);
}