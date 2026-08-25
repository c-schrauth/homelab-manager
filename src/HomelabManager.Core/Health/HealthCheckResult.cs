using HomelabManager.Core.Models;

namespace HomelabManager.Core.Health;

/// <summary>
/// Represents the result of a health check.
/// </summary>
/// <param name="Status">Health status of the service.</param>
/// <param name="Duration">Duration since the last contact.</param>
/// <param name="ErrorMessage">The error message.</param>
public sealed record HealthCheckResult(ServiceStatus Status, TimeSpan Duration, string? ErrorMessage = null)
{
    /// <summary>
    /// Creates a successful health check result.
    /// </summary>
    /// <param name="duration">Duration since the last contact.</param>
    /// <returns>A health check result.</returns>
    public static HealthCheckResult Healthy(TimeSpan duration)
        => new(ServiceStatus.Healthy, duration);
    
    /// <summary>
    /// Creates a failed health check result.
    /// </summary>
    /// <param name="duration">Duration since the last contact.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>An unhealthy check result.</returns>
    public static HealthCheckResult Unhealthy(TimeSpan duration, string errorMessage)
        => new(ServiceStatus.Unhealthy, duration, errorMessage);
}
