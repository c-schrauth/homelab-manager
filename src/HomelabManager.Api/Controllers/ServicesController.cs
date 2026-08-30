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
    private readonly GetServices _getServices;
    private readonly GetService _getService;
    private readonly CreateService _createService;
    private readonly UpdateService _updateService;
    private readonly DeleteService _deleteService;
    private readonly CheckServiceHealth _checkServiceHealth;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServicesController"/> class.
    /// </summary>
    /// <param name="getServices">Use case for retrieving all services.</param>
    /// <param name="getService">Use case for retrieving a single service.</param>
    /// <param name="createService">Use case for creating a new service.</param>
    /// <param name="updateService">Use case for updating an existing service.</param>
    /// <param name="deleteService">Use case for deleting an existing service.</param>
    /// <param name="checkServiceHealth">Service health check use case.</param>
    public ServicesController(GetServices getServices, GetService getService, CreateService createService, UpdateService updateService, DeleteService deleteService, CheckServiceHealth checkServiceHealth)
    {
        _getServices = getServices;
        _getService = getService;
        _createService = createService;
        _updateService = updateService;
        _deleteService = deleteService;
        _checkServiceHealth = checkServiceHealth;
    }

    /// <summary>
    /// Gets all monitored services.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The monitored services.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var services = await _getServices.ExecuteAsync(cancellationToken);

        return Ok(services.Select(ServiceResponse.FromService));
    }

    /// <summary>
    /// Gets a monitored service by identifier.
    /// </summary>
    /// <param name="id">Service identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The requested service.</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServiceResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var service = await _getService.ExecuteAsync(id, cancellationToken);
        if (service is null)
            return NotFound();
        
        return Ok(ServiceResponse.FromService(service));
    }

    /// <summary>
    /// Creates a new monitored service.
    /// </summary>
    /// <param name="request">Service creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The newly created service.</returns>
    [HttpPost]
    public async Task<ActionResult<ServiceResponse>> Create(CreateServiceRequest request, CancellationToken cancellationToken)
    {
        var service = await _createService.ExecuteAsync(request.Name, request.Endpoint, cancellationToken);

        var response = ServiceResponse.FromService(service);

        return CreatedAtAction(nameof(GetById), new { id = service.Id }, response);
    }

    /// <summary>
    /// Updates an existing monitored service.
    /// </summary>
    /// <param name="id">Service identifier.</param>
    /// <param name="request">Service update request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated service.</returns>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ServiceResponse>> Update(Guid id, UpdateServiceRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var service = await _updateService.ExecuteAsync(id, request.Name, request.Endpoint, cancellationToken);

            return Ok(ServiceResponse.FromService(service));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Deletes a monitored service.
    /// </summary>
    /// <param name="id">Service identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content when the service was deleted.</returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _deleteService.ExecuteAsync(id, cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
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