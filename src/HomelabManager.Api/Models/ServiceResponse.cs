using HomelabManager.Core.Models;

namespace HomelabManager.Api.Models;

/// <summary>
/// Represents a monitored service in an API response.
/// </summary>
public sealed class ServiceResponse
{
    /// <summary>
    /// Gets the identifier of the service.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Gets the display name of the service.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the endpoint of the service.
    /// </summary>
    public required Uri Endpoint { get; init; }

    /// <summary>
    /// Gets the current status of the service.
    /// </summary>
    public ServiceStatus Status { get; init; }

    /// <summary>
    /// Creates a response from a domain service.
    /// </summary>
    /// <param name="service">Domain service.</param>
    /// <returns>The API response.</returns>
    public static ServiceResponse FromService(Service service)
    {
        return new ServiceResponse
        {
            Id = service.Id,
            Name = service.Name,
            Endpoint = service.Endpoint,
            Status = service.Status
        };
    }
}