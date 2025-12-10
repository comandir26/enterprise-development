using Bikes.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;
using Bikes.Application.Interfaces;

namespace Bikes.Api.Host.Controllers;

/// <summary>
/// A class that implements a controller for processing HTTP requests for the BikeService class
/// </summary>
/// <param name="service"></param>
/// <param name="logger"></param>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BikesController(IBikeService service, ILogger<BikesController> logger) : ControllerBase
{
    /// <summary>
    /// Returns all existing objects
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<BikeGetDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<List<BikeGetDto>> GetAllBikes()
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
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while retrieving bikes.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BikeGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<BikeGetDto> GetBike(int id)
    {
        try
        {
            logger.LogInformation("Getting bike with ID {BikeId}", id);
            var bike = service.GetBikeById(id);

            if (bike == null)
            {
                logger.LogWarning("Bike with ID {BikeId} not found", id);
                return Problem(
                    title: "Not Found",
                    detail: $"Bike with ID {id} not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            logger.LogInformation("Retrieved bike with ID {BikeId}", id);
            return Ok(bike);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting bike with ID {BikeId}", id);
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while retrieving the bike.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="bikeDto"></param>
    [HttpPost]
    [ProducesResponseType(typeof(CreatedAtActionResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<CreatedAtActionResult> CreateBike([FromBody] BikeCreateUpdateDto bikeDto)
    {
        try
        {
            logger.LogInformation("Creating new bike with serial number {SerialNumber}", bikeDto.SerialNumber);

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid bike data: {ModelErrors}",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));

                return ValidationProblem(
                    title: "Validation Error",
                    detail: "One or more validation errors occurred.",
                    modelStateDictionary: ModelState);
            }

            var id = service.CreateBike(bikeDto);
            logger.LogInformation("Created bike with ID {BikeId}", id);

            return CreatedAtAction(
                nameof(GetBike),
                new { id },
                new { id, message = "Bike created successfully." });
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Error creating bike: {ErrorMessage}", ex.Message);
            return Problem(
                title: "Bad Request",
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating bike");
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while creating the bike.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id"></param>
    /// <param name="bikeDto"></param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BikeGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<BikeGetDto> UpdateBike(int id, [FromBody] BikeCreateUpdateDto bikeDto)
    {
        try
        {
            logger.LogInformation("Updating bike with ID {BikeId}", id);

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid bike data for update: {ModelErrors}",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));

                return ValidationProblem(
                    title: "Validation Error",
                    detail: "One or more validation errors occurred.",
                    modelStateDictionary: ModelState);
            }

            var bike = service.UpdateBike(id, bikeDto);
            if (bike == null)
            {
                logger.LogWarning("Bike with ID {BikeId} not found for update", id);
                return Problem(
                    title: "Not Found",
                    detail: $"Bike with ID {id} not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            logger.LogInformation("Updated bike with ID {BikeId}", id);
            return Ok(bike);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Error updating bike: {ErrorMessage}", ex.Message);
            return Problem(
                title: "Bad Request",
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating bike with ID {BikeId}", id);
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while updating the bike.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult DeleteBike(int id)
    {
        try
        {
            logger.LogInformation("Deleting bike with ID {BikeId}", id);
            var result = service.DeleteBike(id);

            if (!result)
            {
                logger.LogWarning("Bike with ID {BikeId} not found for deletion", id);
                return Problem(
                    title: "Not Found",
                    detail: $"Bike with ID {id} not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            logger.LogInformation("Deleted bike with ID {BikeId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting bike with ID {BikeId}", id);
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while deleting the bike.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}