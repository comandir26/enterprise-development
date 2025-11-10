using Bikes.Application.Dto;
using Bikes.Domain.Models;

namespace Bikes.Application.Services;

/// <summary>
/// Interface for the Renter service class
/// </summary>
public interface IRenterService
{
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="renterDto">DTO object</param>
    /// <returns>ID of the created object</returns>
    public int CreateRenter(RenterDto renterDto);

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<Renter> GetAllRenters();

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Renter? GetRenterById(int id);

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="renterDto">DTO object</param>
    /// <returns>Object if exist</returns>
    public Renter? UpdateRenter(int id, RenterDto renterDto);

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool DeleteRenter(int id);
}