using Bikes.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;
using Bikes.Application.Interfaces;

namespace Bikes.Api.Host.Controllers;

/// <summary>
/// A class that implements a controller for processing HTTP requests for the RentService class
/// </summary>
/// <param name="service"></param>
/// <param name="logger"></param>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RentsController(IRentService service, ILogger<RentsController> logger) : ControllerBase
{
    /// <summary>
    /// Returns all existing objects
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<RentGetDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<List<RentGetDto>> GetAllRents()
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
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while retrieving rents.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RentGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<RentGetDto> GetRent(int id)
    {
        try
        {
            logger.LogInformation("Getting rent with ID {RentId}", id);
            var rent = service.GetRentById(id);

            if (rent == null)
            {
                logger.LogWarning("Rent with ID {RentId} not found", id);
                return Problem(
                    title: "Not Found",
                    detail: $"Rent with ID {id} not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            return Ok(rent);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting rent with ID {RentId}", id);
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while retrieving the rent.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="rentDto"></param>
    [HttpPost]
    [ProducesResponseType(typeof(RentGetDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<RentGetDto> CreateRent([FromBody] RentCreateUpdateDto rentDto)
    {
        try
        {
            logger.LogInformation("Creating new rent for bike {BikeId} by renter {RenterId}",
                rentDto.BikeId, rentDto.RenterId);

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid rent data: {ModelErrors}",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));

                return ValidationProblem(
                    title: "Validation Error",
                    detail: "One or more validation errors occurred.",
                    modelStateDictionary: ModelState);
            }

            var createdRent = service.CreateRent(rentDto);

            if (createdRent == null)
            {
                logger.LogError("Failed to create rent");
                return Problem(
                    title: "Internal Server Error",
                    detail: "Failed to create rent.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }

            logger.LogInformation("Created rent with ID {RentId}", createdRent.Id);

            return CreatedAtAction(
                nameof(GetRent),
                new { id = createdRent.Id },
                createdRent);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Error creating rent: {ErrorMessage}", ex.Message);
            return Problem(
                title: "Bad Request",
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating rent");
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while creating the rent.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id"></param>
    /// <param name="rentDto"></param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(RentGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<RentGetDto> UpdateRent(int id, [FromBody] RentCreateUpdateDto rentDto)
    {
        try
        {
            logger.LogInformation("Updating rent with ID {RentId}", id);

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid rent data for update: {ModelErrors}",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));

                return ValidationProblem(
                    title: "Validation Error",
                    detail: "One or more validation errors occurred.",
                    modelStateDictionary: ModelState);
            }

            var rent = service.UpdateRent(id, rentDto);
            if (rent == null)
            {
                logger.LogWarning("Rent with ID {RentId} not found for update", id);
                return Problem(
                    title: "Not Found",
                    detail: $"Rent with ID {id} not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            logger.LogInformation("Updated rent with ID {RentId}", id);
            return Ok(rent);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Error updating rent: {ErrorMessage}", ex.Message);
            return Problem(
                title: "Bad Request",
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating rent with ID {RentId}", id);
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while updating the rent.",
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
    public ActionResult DeleteRent(int id)
    {
        try
        {
            logger.LogInformation("Deleting rent with ID {RentId}", id);
            var result = service.DeleteRent(id);

            if (!result)
            {
                logger.LogWarning("Rent with ID {RentId} not found for deletion", id);
                return Problem(
                    title: "Not Found",
                    detail: $"Rent with ID {id} not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            logger.LogInformation("Deleted rent with ID {RentId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting rent with ID {RentId}", id);
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while deleting the rent.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}