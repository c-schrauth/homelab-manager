namespace HomelabManager.Core.Health;

/// <summary>
/// Defines a mechanism for checking the health of a service.
/// </summary>
public interface IHealthChecker
{
    /// <summary>
    /// Checks the health of the specified endpoint.
    /// </summary>
    /// <param name="endpoint">Uri to the endpoint to check.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<HealthCheckResult> CheckAsync(Uri endpoint, CancellationToken cancellationToken = default);
}