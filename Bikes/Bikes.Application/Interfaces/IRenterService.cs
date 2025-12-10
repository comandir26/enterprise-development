using Bikes.Contracts.Dto;

namespace Bikes.Application.Interfaces;

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
    public int CreateRenter(RenterCreateUpdateDto renterDto);

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<RenterGetDto> GetAllRenters();

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public RenterGetDto? GetRenterById(int id);

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="renterDto">DTO object</param>
    /// <returns>Object if exist</returns>
    public RenterGetDto? UpdateRenter(int id, RenterCreateUpdateDto renterDto);

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool DeleteRenter(int id);
}