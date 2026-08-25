namespace HomelabManager.Core.Models;

/// <summary>
/// Represents a service monitored by Homelab Manager.
/// </summary>
public sealed class Service
{
    /// <summary>
    /// Gets or sets the unique identifier of the service.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the display name of the service.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Get or sets the endpoint used to check the service.
    /// </summary>
    public required Uri Endpoint { get; set; }

    /// <summary>
    /// Gets or sets the current status of the service.
    /// </summary>
    public ServiceStatus Status { get; set; } = ServiceStatus.Unknown;
}