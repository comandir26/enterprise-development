using Bikes.Contracts.Dto;
using Bikes.Domain.Models;

namespace Bikes.Application.Services;

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
    public int CreateBike(BikeDto bikeDto);

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<BikeDto> GetAllBikes();

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BikeDto? GetBikeById(int id);

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="bikeDto">DTO object</param>
    /// <returns>Object if exist</returns>
    public BikeDto? UpdateBike(int id, BikeDto bikeDto);

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool DeleteBike(int id);
}