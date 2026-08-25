using HomelabManager.Api.Models;
using HomelabManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomelabManager.Api.Controllers;

/// <summary>
/// Provides operations for monitored services.
/// </summary>
[ApiController]
[Route("api/services")]
public sealed class ServicesController : ControllerBase
{
    private readonly CheckServiceHealth _checkServiceHealth;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServicesController"/> class.
    /// </summary>
    /// <param name="checkServiceHealth">Service health check use case.</param>
    public ServicesController(CheckServiceHealth checkServiceHealth)
    {
        _checkServiceHealth = checkServiceHealth;
    }

    /// <summary>
    /// Checks the health of a service.
    /// </summary>
    /// <param name="id">Service identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The health check result.</returns>
    [HttpGet("{id:guid}/health")]
    public async Task<IActionResult> CheckHealth(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _checkServiceHealth.ExecuteAsync(id, cancellationToken);
            var response = new HealthCheckResponse(result.Status.ToString(), result.Duration, result.ErrorMessage);

            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}