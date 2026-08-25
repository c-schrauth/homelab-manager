using HomelabManager.Core.Models;
using HomelabManager.Core.Health;

namespace HomelabManager.Application.Services;

/// <summary>
/// Checks the health of a service.
/// </summary>
public sealed class CheckServiceHealth
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IHealthChecker _healthChecker;

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckServiceHealth"/> class.
    /// </summary>
    /// <param name="serviceRepository">Repository used to retrieve the service.</param>
    /// <param name="healthChecker">Health checker used to determine service availability.</param>
    public CheckServiceHealth(IServiceRepository serviceRepository, IHealthChecker healthChecker)
    {
        _serviceRepository = serviceRepository;
        _healthChecker = healthChecker;
    }

    /// <summary>
    /// Checks the specified service and returns the result.
    /// </summary>
    /// <param name="serviceId">Service to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The health check result.</returns>
    public async Task<HealthCheckResult> ExecuteAsync(Guid serviceId, CancellationToken cancellationToken = default)
    {
        var service = await _serviceRepository.GetByIdAsync(serviceId, cancellationToken);
        if (service is null)
            throw new KeyNotFoundException($"Service '{serviceId}' was not found.");

        var result = await _healthChecker.CheckAsync(service.Endpoint, cancellationToken);

        service.Status = result.Status;

        return result;
    }
}