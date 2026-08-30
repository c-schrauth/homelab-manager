namespace HomelabManager.Api.Models;

/// <summary>
/// Request used to create a monitored service.
/// </summary>
public sealed class CreateServiceRequest
{
    /// <summary>
    /// Gets or sets the display name of the service.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the endpoint of the service.
    /// </summary>
    public required Uri Endpoint { get; set; }
}