using Bikes.Application.Dto;
using Bikes.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bikes.Api.Host.Controllers;

/// <summary>
/// A class that implements a controller for processing HTTP requests for the BikeService class
/// </summary>
/// <param name="service"></param>
/// <param name="logger"></param>
[ApiController]
[Route("api/[controller]")]
public class BikesController(IBikeService service, ILogger<BikesController> logger) : ControllerBase
{
    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetAllBikes()
    {
        try
        {
            logger.LogInformation("Getting all bikes");
            var bikes = service.GetAllBikes();
            logger.LogInformation("Retrieved {Count} bikes", bikes.Count);
            return Ok(bikes);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all bikes");
            return StatusCode(500, new { error = "An error occurred while retrieving bikes." });
        }
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public IActionResult GetBike(int id)
    {
        try
        {
            logger.LogInformation("Getting bike with ID {BikeId}", id);
            var bike = service.GetBikeById(id);

            if (bike == null)
            {
                logger.LogWarning("Bike with ID {BikeId} not found", id);
                return NotFound(new { error = $"Bike with ID {id} not found." });
            }

            logger.LogInformation("Retrieved bike with ID {BikeId}", id);
            return Ok(bike);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting bike with ID {BikeId}", id);
            return StatusCode(500, new { error = "An error occurred while retrieving the bike." });
        }
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="bikeDto"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateBike([FromBody] BikeDto bikeDto)
    {
        try
        {
            logger.LogInformation("Creating new bike with serial number {SerialNumber}", bikeDto.SerialNumber);

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid bike data: {ModelErrors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            var id = service.CreateBike(bikeDto);
            logger.LogInformation("Created bike with ID {BikeId}", id);

            return CreatedAtAction(nameof(GetBike), new { id }, new { id, message = "Bike created successfully." });
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Error creating bike: {ErrorMessage}", ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating bike");
            return StatusCode(500, new { error = "An error occurred while creating the bike." });
        }
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id"></param>
    /// <param name="bikeDto"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    public IActionResult UpdateBike(int id, [FromBody] BikeDto bikeDto)
    {
        try
        {
            logger.LogInformation("Updating bike with ID {BikeId}", id);

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid bike data for update: {ModelErrors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            var bike = service.UpdateBike(id, bikeDto);
            if (bike == null)
            {
                logger.LogWarning("Bike with ID {BikeId} not found for update", id);
                return NotFound(new { error = $"Bike with ID {id} not found." });
            }

            logger.LogInformation("Updated bike with ID {BikeId}", id);
            return Ok(new { message = "Bike updated successfully.", bike });
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Error updating bike: {ErrorMessage}", ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating bike with ID {BikeId}", id);
            return StatusCode(500, new { error = "An error occurred while updating the bike." });
        }
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public IActionResult DeleteBike(int id)
    {
        try
        {
            logger.LogInformation("Deleting bike with ID {BikeId}", id);
            var result = service.DeleteBike(id);

            if (!result)
            {
                logger.LogWarning("Bike with ID {BikeId} not found for deletion", id);
                return NotFound(new { error = $"Bike with ID {id} not found." });
            }

            logger.LogInformation("Deleted bike with ID {BikeId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting bike with ID {BikeId}", id);
            return StatusCode(500, new { error = "An error occurred while deleting the bike." });
        }
    }
}