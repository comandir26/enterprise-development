using Bikes.Application.Dto;
using Bikes.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bikes.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BikeModelsController(IBikeModelService service, ILogger<BikeModelsController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllBikeModels()
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
            return StatusCode(500, new { error = "An error occurred while retrieving bike models." });
        }
    }

    [HttpGet("{id:int}")]
    public IActionResult GetBikeModel(int id)
    {
        try
        {
            logger.LogInformation("Getting bike model with ID {ModelId}", id);
            var model = service.GetBikeModelById(id);

            if (model == null)
            {
                logger.LogWarning("Bike model with ID {ModelId} not found", id);
                return NotFound(new { error = $"Bike model with ID {id} not found." });
            }

            return Ok(model);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting bike model with ID {ModelId}", id);
            return StatusCode(500, new { error = "An error occurred while retrieving the bike model." });
        }
    }

    [HttpPost]
    public IActionResult CreateBikeModel([FromBody] BikeModelDto bikeModelDto)
    {
        try
        {
            logger.LogInformation("Creating new bike model of type {BikeType}", bikeModelDto.Type);

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid bike model data: {ModelErrors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            var id = service.CreateBikeModel(bikeModelDto);
            logger.LogInformation("Created bike model with ID {ModelId}", id);

            return CreatedAtAction(nameof(GetBikeModel), new { id }, new { id, message = "Bike model created successfully." });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating bike model");
            return StatusCode(500, new { error = "An error occurred while creating the bike model." });
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateBikeModel(int id, [FromBody] BikeModelDto bikeModelDto)
    {
        try
        {
            logger.LogInformation("Updating bike model with ID {ModelId}", id);

            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            var model = service.UpdateBikeModel(id, bikeModelDto);
            if (model == null)
            {
                logger.LogWarning("Bike model with ID {ModelId} not found for update", id);
                return NotFound(new { error = $"Bike model with ID {id} not found." });
            }

            logger.LogInformation("Updated bike model with ID {ModelId}", id);
            return Ok(new { message = "Bike model updated successfully.", model });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating bike model with ID {ModelId}", id);
            return StatusCode(500, new { error = "An error occurred while updating the bike model." });
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteBikeModel(int id)
    {
        try
        {
            logger.LogInformation("Deleting bike model with ID {ModelId}", id);
            var result = service.DeleteBikeModel(id);

            if (!result)
            {
                logger.LogWarning("Bike model with ID {ModelId} not found for deletion", id);
                return NotFound(new { error = $"Bike model with ID {id} not found." });
            }

            logger.LogInformation("Deleted bike model with ID {ModelId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting bike model with ID {ModelId}", id);
            return StatusCode(500, new { error = "An error occurred while deleting the bike model." });
        }
    }
}