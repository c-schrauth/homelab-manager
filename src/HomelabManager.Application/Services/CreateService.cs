using HomelabManager.Core.Models;

namespace HomelabManager.Application.Services;

/// <summary>
/// Creates a monitored service.
/// </summary>
public sealed class CreateService
{
    private readonly IServiceRepository _serviceRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateService"/> class.
    /// </summary>
    /// <param name="serviceRepository">Repository used to persist the service.</param>
    public CreateService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    /// <summary>
    /// Creates a new monitored service.
    /// </summary>
    /// <param name="name">Display name of the service.</param>
    /// <param name="endpoint">Endpoint used to check the service.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The newly created service.</returns>
    public async Task<Service> ExecuteAsync(string name, Uri endpoint, CancellationToken cancellationToken = default)
    {
        var service = new Service
        {
            Id = Guid.NewGuid(),
            Name = name,
            Endpoint = endpoint,
            Status = ServiceStatus.Unknown
        };

        await _serviceRepository.AddAsync(service, cancellationToken);

        return service;
    }
}