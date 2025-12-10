using Bikes.Contracts.Dto;

namespace Bikes.Application.Interfaces;

/// <summary>
/// Interface for the Rent service class
/// </summary>
public interface IRentService
{
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="rentDto">DTO object</param>
    /// <returns>ID of the created object</returns>
    public int CreateRent(RentCreateUpdateDto rentDto);

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<RentGetDto> GetAllRents();

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public RentGetDto? GetRentById(int id);

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="rentDto">DTO object</param>
    /// <returns>Object if exist</returns>
    public RentGetDto? UpdateRent(int id, RentCreateUpdateDto rentDto);

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool DeleteRent(int id);
}