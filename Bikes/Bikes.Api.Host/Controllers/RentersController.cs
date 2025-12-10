using Bikes.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;
using Bikes.Application.Interfaces;

namespace Bikes.Api.Host.Controllers;

/// <summary>
/// A class that implements a controller for processing HTTP requests for the RenterService class
/// </summary>
/// <param name="service"></param>
/// <param name="logger"></param>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RentersController(IRenterService service, ILogger<RentersController> logger) : ControllerBase
{
    /// <summary>
    /// Returns all existing objects
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<RenterGetDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<List<RenterGetDto>> GetAllRenters()
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
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while retrieving renters.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RenterGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<RenterGetDto> GetRenter(int id)
    {
        try
        {
            logger.LogInformation("Getting renter with ID {RenterId}", id);
            var renter = service.GetRenterById(id);

            if (renter == null)
            {
                logger.LogWarning("Renter with ID {RenterId} not found", id);
                return Problem(
                    title: "Not Found",
                    detail: $"Renter with ID {id} not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            return Ok(renter);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting renter with ID {RenterId}", id);
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while retrieving the renter.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="renterDto"></param>
    [HttpPost]
    [ProducesResponseType(typeof(CreatedAtActionResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<CreatedAtActionResult> CreateRenter([FromBody] RenterCreateUpdateDto renterDto)
    {
        try
        {
            logger.LogInformation("Creating new renter: {FullName}", renterDto.FullName);

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid renter data: {ModelErrors}",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));

                return ValidationProblem(
                    title: "Validation Error",
                    detail: "One or more validation errors occurred.",
                    modelStateDictionary: ModelState);
            }

            var id = service.CreateRenter(renterDto);
            logger.LogInformation("Created renter with ID {RenterId}", id);

            return CreatedAtAction(
                nameof(GetRenter),
                new { id },
                new { id, message = "Renter created successfully." });
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Error creating renter: {ErrorMessage}", ex.Message);
            return Problem(
                title: "Bad Request",
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating renter");
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while creating the renter.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id"></param>
    /// <param name="renterDto"></param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(RenterGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<RenterGetDto> UpdateRenter(int id, [FromBody] RenterCreateUpdateDto renterDto)
    {
        try
        {
            logger.LogInformation("Updating renter with ID {RenterId}", id);

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid renter data for update: {ModelErrors}",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));

                return ValidationProblem(
                    title: "Validation Error",
                    detail: "One or more validation errors occurred.",
                    modelStateDictionary: ModelState);
            }

            var renter = service.UpdateRenter(id, renterDto);
            if (renter == null)
            {
                logger.LogWarning("Renter with ID {RenterId} not found for update", id);
                return Problem(
                    title: "Not Found",
                    detail: $"Renter with ID {id} not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            logger.LogInformation("Updated renter with ID {RenterId}", id);
            return Ok(renter);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Error updating renter: {ErrorMessage}", ex.Message);
            return Problem(
                title: "Bad Request",
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating renter with ID {RenterId}", id);
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while updating the renter.",
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
    public ActionResult DeleteRenter(int id)
    {
        try
        {
            logger.LogInformation("Deleting renter with ID {RenterId}", id);
            var result = service.DeleteRenter(id);

            if (!result)
            {
                logger.LogWarning("Renter with ID {RenterId} not found for deletion", id);
                return Problem(
                    title: "Not Found",
                    detail: $"Renter with ID {id} not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            logger.LogInformation("Deleted renter with ID {RenterId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting renter with ID {RenterId}", id);
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while deleting the renter.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}