using Bikes.Application.Dto;
using Bikes.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bikes.Api.Host.Controllers;

/// <summary>
/// A class that implements a controller for processing HTTP requests for the RentService class
/// </summary>
/// <param name="service"></param>
/// <param name="logger"></param>
[ApiController]
[Route("api/[controller]")]
public class RentsController(IRentService service, ILogger<RentsController> logger) : ControllerBase
{
    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetAllRents()
    {
        try
        {
            logger.LogInformation("Getting all rents");
            var rents = service.GetAllRents();
            logger.LogInformation("Retrieved {Count} rents", rents.Count);
            return Ok(rents);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all rents");
            return StatusCode(500, new { error = "An error occurred while retrieving rents." });
        }
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public IActionResult GetRent(int id)
    {
        try
        {
            logger.LogInformation("Getting rent with ID {RentId}", id);
            var rent = service.GetRentById(id);

            if (rent == null)
            {
                logger.LogWarning("Rent with ID {RentId} not found", id);
                return NotFound(new { error = $"Rent with ID {id} not found." });
            }

            return Ok(rent);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting rent with ID {RentId}", id);
            return StatusCode(500, new { error = "An error occurred while retrieving the rent." });
        }
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="rentDto"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateRent([FromBody] RentDto rentDto)
    {
        try
        {
            logger.LogInformation("Creating new rent for bike {BikeId} by renter {RenterId}",
                rentDto.BikeId, rentDto.RenterId);

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid rent data: {ModelErrors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            var id = service.CreateRent(rentDto);
            logger.LogInformation("Created rent with ID {RentId}", id);

            return CreatedAtAction(nameof(GetRent), new { id }, new { id, message = "Rent created successfully." });
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Error creating rent: {ErrorMessage}", ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating rent");
            return StatusCode(500, new { error = "An error occurred while creating the rent." });
        }
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id"></param>
    /// <param name="rentDto"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    public IActionResult UpdateRent(int id, [FromBody] RentDto rentDto)
    {
        try
        {
            logger.LogInformation("Updating rent with ID {RentId}", id);

            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            var rent = service.UpdateRent(id, rentDto);
            if (rent == null)
            {
                logger.LogWarning("Rent with ID {RentId} not found for update", id);
                return NotFound(new { error = $"Rent with ID {id} not found." });
            }

            logger.LogInformation("Updated rent with ID {RentId}", id);
            return Ok(new { message = "Rent updated successfully.", rent });
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Error updating rent: {ErrorMessage}", ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating rent with ID {RentId}", id);
            return StatusCode(500, new { error = "An error occurred while updating the rent." });
        }
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public IActionResult DeleteRent(int id)
    {
        try
        {
            logger.LogInformation("Deleting rent with ID {RentId}", id);
            var result = service.DeleteRent(id);

            if (!result)
            {
                logger.LogWarning("Rent with ID {RentId} not found for deletion", id);
                return NotFound(new { error = $"Rent with ID {id} not found." });
            }

            logger.LogInformation("Deleted rent with ID {RentId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting rent with ID {RentId}", id);
            return StatusCode(500, new { error = "An error occurred while deleting the rent." });
        }
    }
}