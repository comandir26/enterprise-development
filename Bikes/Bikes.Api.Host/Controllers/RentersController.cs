using Bikes.Application.Dto;
using Bikes.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bikes.Api.Host.Controllers;

/// <summary>
/// A class that implements a controller for processing HTTP requests for the RenterService class
/// </summary>
/// <param name="service"></param>
/// <param name="logger"></param>
[ApiController]
[Route("api/[controller]")]
public class RentersController(IRenterService service, ILogger<RentersController> logger) : ControllerBase
{
    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetAllRenters()
    {
        try
        {
            logger.LogInformation("Getting all renters");
            var renters = service.GetAllRenters();
            logger.LogInformation("Retrieved {Count} renters", renters.Count);
            return Ok(renters);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all renters");
            return StatusCode(500, new { error = "An error occurred while retrieving renters." });
        }
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public IActionResult GetRenter(int id)
    {
        try
        {
            logger.LogInformation("Getting renter with ID {RenterId}", id);
            var renter = service.GetRenterById(id);

            if (renter == null)
            {
                logger.LogWarning("Renter with ID {RenterId} not found", id);
                return NotFound(new { error = $"Renter with ID {id} not found." });
            }

            return Ok(renter);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting renter with ID {RenterId}", id);
            return StatusCode(500, new { error = "An error occurred while retrieving the renter." });
        }
    }

    /// <summary>
    /// Creates a new objec
    /// </summary>
    /// <param name="renterDto"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateRenter([FromBody] RenterDto renterDto)
    {
        try
        {
            logger.LogInformation("Creating new renter: {FullName}", renterDto.FullName);

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid renter data: {ModelErrors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            var id = service.CreateRenter(renterDto);
            logger.LogInformation("Created renter with ID {RenterId}", id);

            return CreatedAtAction(nameof(GetRenter), new { id }, new { id, message = "Renter created successfully." });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating renter");
            return StatusCode(500, new { error = "An error occurred while creating the renter." });
        }
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id"></param>
    /// <param name="renterDto"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    public IActionResult UpdateRenter(int id, [FromBody] RenterDto renterDto)
    {
        try
        {
            logger.LogInformation("Updating renter with ID {RenterId}", id);

            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            var renter = service.UpdateRenter(id, renterDto);
            if (renter == null)
            {
                logger.LogWarning("Renter with ID {RenterId} not found for update", id);
                return NotFound(new { error = $"Renter with ID {id} not found." });
            }

            logger.LogInformation("Updated renter with ID {RenterId}", id);
            return Ok(new { message = "Renter updated successfully.", renter });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating renter with ID {RenterId}", id);
            return StatusCode(500, new { error = "An error occurred while updating the renter." });
        }
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public IActionResult DeleteRenter(int id)
    {
        try
        {
            logger.LogInformation("Deleting renter with ID {RenterId}", id);
            var result = service.DeleteRenter(id);

            if (!result)
            {
                logger.LogWarning("Renter with ID {RenterId} not found for deletion", id);
                return NotFound(new { error = $"Renter with ID {id} not found." });
            }

            logger.LogInformation("Deleted renter with ID {RenterId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting renter with ID {RenterId}", id);
            return StatusCode(500, new { error = "An error occurred while deleting the renter." });
        }
    }
}