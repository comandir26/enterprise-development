using Bikes.Application.Services;
using Bikes.Contracts.Dto;
using Bikes.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bikes.Api.Host.Controllers;

/// <summary>
/// A class that implements a controller for processing HTTP requests for the AnalyticsService class
/// </summary>
/// <param name="service"></param>
/// <param name="logger"></param>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AnalyticsController(
    IAnalyticsService service,
    ILogger<AnalyticsController> logger) : ControllerBase
{
    /// <summary>
    /// A method that returns information about all sports bikes
    /// </summary>
    [HttpGet("sport-bikes")]
    [ProducesResponseType(typeof(List<BikeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<List<BikeDto>> GetSportBikes()
    {
        try
        {
            logger.LogInformation("Getting sport bikes");
            var bikes = service.GetSportBikes(); 
            logger.LogInformation("Retrieved {Count} sport bikes", bikes.Count);
            return Ok(bikes); 
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting sport bikes");
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while retrieving sport bikes.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// A method that returns the top 5 bike models by rental duration
    /// </summary>
    [HttpGet("top-models/duration")]
    [ProducesResponseType(typeof(List<BikeModelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<List<BikeModelDto>> GetTopModelsByDuration()
    {
        try
        {
            logger.LogInformation("Getting top models by rent duration");
            var models = service.GetTopFiveModelsByRentDuration();
            logger.LogInformation("Retrieved top {Count} models by duration", models.Count);
            return Ok(models);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting top models by duration");
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while retrieving top models by duration.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// A method that returns the top 5 bike models in terms of rental income
    /// </summary>
    [HttpGet("top-models/profit")]
    [ProducesResponseType(typeof(List<BikeModelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<List<BikeModelDto>> GetTopModelsByProfit()
    {
        try
        {
            logger.LogInformation("Getting top models by profit");
            var models = service.GetTopFiveModelsByProfit(); 
            logger.LogInformation("Retrieved top {Count} models by profit", models.Count);
            return Ok(models);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting top models by profit");
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while retrieving top models by profit.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// A method that returns information about the minimum, maximum, and average bike rental time.
    /// </summary>
    [HttpGet("stats/duration")]
    [ProducesResponseType(typeof(RentalDurationStatsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<RentalDurationStatsDto> GetRentalDurationStats()
    {
        try
        {
            logger.LogInformation("Getting rental duration statistics");
            var stats = service.GetRentalDurationStats();
            logger.LogInformation("Retrieved rental duration stats: Min={Min}, Max={Max}, Avg={Avg}",
                stats.Min, stats.Max, stats.Average);

            return Ok(stats);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting rental duration statistics");
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while retrieving rental duration statistics.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// A method that returns the total rental time of each type of bike
    /// </summary>
    [HttpGet("stats/rental-time-by-type")]
    [ProducesResponseType(typeof(Dictionary<BikeType, int>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<Dictionary<BikeType, int>> GetTotalRentalTimeByType()
    {
        try
        {
            logger.LogInformation("Getting total rental time by bike type");
            var stats = service.GetTotalRentalTimeByType();
            logger.LogInformation("Retrieved rental time by type for {Count} bike types", stats.Count);
            return Ok(stats);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting total rental time by type");
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while retrieving total rental time by type.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// A method that returns information about the customers who have rented bicycles the most times.
    /// </summary>
    [HttpGet("top-renters")]
    [ProducesResponseType(typeof(List<RenterDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<List<RenterDto>> GetTopRenters()
    {
        try
        {
            logger.LogInformation("Getting top renters");
            var renters = service.GetTopThreeRenters(); 
            logger.LogInformation("Retrieved top {Count} renters", renters.Count);
            return Ok(renters); 
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting top renters");
            return Problem(
                title: "Internal Server Error",
                detail: "An error occurred while retrieving top renters.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}