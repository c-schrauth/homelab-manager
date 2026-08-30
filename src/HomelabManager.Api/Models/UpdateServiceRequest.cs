namespace HomelabManager.Api.Models;

/// <summary>
/// Request used to update a monitored service.
/// </summary>
public sealed class UpdateServiceRequest
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