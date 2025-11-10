using Bikes.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bikes.Api.Host.Controllers;

/// <summary>
/// A class that implements a controller for processing HTTP requests for the AnalyticsService class
/// </summary>
/// <param name="service"></param>
/// <param name="logger"></param>
[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(IAnalyticsService service, ILogger<AnalyticsController> logger) : ControllerBase
{
    /// <summary>
    /// A method that returns information about all sports bikes
    /// </summary>
    [HttpGet("sport-bikes")]
    public IActionResult GetSportBikes()
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
            return StatusCode(500, new { error = "An error occurred while retrieving sport bikes." });
        }
    }

    /// <summary>
    /// A method that returns the top 5 bike models by rental duration
    /// </summary>
    [HttpGet("top-models/duration")]
    public IActionResult GetTopModelsByDuration()
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
            return StatusCode(500, new { error = "An error occurred while retrieving top models by duration." });
        }
    }

    /// <summary>
    /// A method that returns the top 5 bike models in terms of rental income
    /// </summary>
    [HttpGet("top-models/profit")]
    public IActionResult GetTopModelsByProfit()
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
            return StatusCode(500, new { error = "An error occurred while retrieving top models by profit." });
        }
    }

    /// <summary>
    /// A method that returns information about the minimum, maximum, and average bike rental time.
    /// </summary>
    [HttpGet("stats/duration")]
    public IActionResult GetRentalDurationStats()
    {
        try
        {
            logger.LogInformation("Getting rental duration statistics");
            var stats = service.GetRentalDurationStats();
            logger.LogInformation("Retrieved rental duration stats: Min={Min}, Max={Max}, Avg={Avg}",
                stats.min, stats.max, stats.avg);
            return Ok(new
            {
                min = stats.min,
                max = stats.max,
                average = stats.avg
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting rental duration statistics");
            return StatusCode(500, new { error = "An error occurred while retrieving rental duration statistics." });
        }
    }

    /// <summary>
    /// A method that returns the total rental time of each type of bike
    /// </summary>
    [HttpGet("stats/rental-time-by-type")]
    public IActionResult GetTotalRentalTimeByType()
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
            return StatusCode(500, new { error = "An error occurred while retrieving total rental time by type." });
        }
    }

    /// <summary>
    /// A method that returns information about the customers who have rented bicycles the most times.
    /// </summary>
    [HttpGet("top-renters")]
    public IActionResult GetTopRenters()
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
            return StatusCode(500, new { error = "An error occurred while retrieving top renters." });
        }
    }
}