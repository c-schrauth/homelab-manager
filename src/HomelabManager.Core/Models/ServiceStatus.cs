namespace HomelabManager.Core.Models;

/// <summary>
/// Represents the current health state of a homelab service.
/// </summary>
public enum ServiceStatus
{
    /// <summary>
    /// Service status is unknown.
    /// </summary>
    Unknown,
    /// <summary>
    /// Service status is healthy.
    /// </summary>
    Healthy,
    /// <summary>
    /// Service status is unhealthy.
    /// </summary>
    Unhealthy
}

