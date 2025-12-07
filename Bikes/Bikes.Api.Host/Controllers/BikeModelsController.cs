using Bikes.Contracts.Dto;
using Bikes.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bikes.Api.Host.Controllers;

/// <summary>
/// A class that implements a controller for processing HTTP requests for the BikeModels class
/// </summary>
/// <param name="service"></param>
/// <param name="logger"></param>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BikeModelsController(IBikeModelService service, ILogger<BikeModelsController> logger) : ControllerBase
{
    /// <summary>
    /// Returns all existing objects
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<BikeModelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<List<BikeModelDto>> GetAllBikeModels()
    {
        try
        {
            logger.LogInformation("Getting all bike models");
            var models = service.GetAllBikeModels();
            logger.LogInformation("Retrieved {Count} bike models", models.Count);
            return Ok(models);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all bike models");
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while retrieving bike models.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BikeModelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<BikeModelDto> GetBikeModel(int id)
    {
        try
        {
            logger.LogInformation("Getting bike model with ID {ModelId}", id);
            var model = service.GetBikeModelById(id);

            if (model == null)
            {
                logger.LogWarning("Bike model with ID {ModelId} not found", id);
                return Problem(
                    title: "Not Found",
                    detail: $"Bike model with ID {id} not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            return Ok(model);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting bike model with ID {ModelId}", id);
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while retrieving the bike model.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="bikeModelDto"></param>
    [HttpPost]
    [ProducesResponseType(typeof(CreatedAtActionResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<CreatedAtActionResult> CreateBikeModel([FromBody] BikeModelDto bikeModelDto)
    {
        try
        {
            logger.LogInformation("Creating new bike model of type {BikeType}", bikeModelDto.Type);

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid bike model data: {ModelErrors}",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));

                return ValidationProblem(
                    title: "Validation Error",
                    detail: "One or more validation errors occurred.",
                    modelStateDictionary: ModelState);
            }

            var id = service.CreateBikeModel(bikeModelDto);
            logger.LogInformation("Created bike model with ID {ModelId}", id);

            return CreatedAtAction(
                nameof(GetBikeModel),
                new { id },
                new { id, message = "Bike model created successfully." });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating bike model");
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while creating the bike model.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id"></param>
    /// <param name="bikeModelDto"></param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BikeModelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<BikeModelDto> UpdateBikeModel(int id, [FromBody] BikeModelDto bikeModelDto)
    {
        try
        {
            logger.LogInformation("Updating bike model with ID {ModelId}", id);

            if (!ModelState.IsValid)
            {
                return ValidationProblem(
                    title: "Validation Error",
                    detail: "One or more validation errors occurred.",
                    modelStateDictionary: ModelState);
            }

            var model = service.UpdateBikeModel(id, bikeModelDto);
            if (model == null)
            {
                logger.LogWarning("Bike model with ID {ModelId} not found for update", id);
                return Problem(
                    title: "Not Found",
                    detail: $"Bike model with ID {id} not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            logger.LogInformation("Updated bike model with ID {ModelId}", id);
            return Ok(model);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating bike model with ID {ModelId}", id);
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while updating the bike model.",
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
    public ActionResult DeleteBikeModel(int id)
    {
        try
        {
            logger.LogInformation("Deleting bike model with ID {ModelId}", id);
            var result = service.DeleteBikeModel(id);

            if (!result)
            {
                logger.LogWarning("Bike model with ID {ModelId} not found for deletion", id);
                return Problem(
                    title: "Not Found",
                    detail: $"Bike model with ID {id} not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            logger.LogInformation("Deleted bike model with ID {ModelId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting bike model with ID {ModelId}", id);
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while deleting the bike model.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}