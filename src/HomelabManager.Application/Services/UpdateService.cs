using HomelabManager.Core.Models;

namespace HomelabManager.Application.Services;

/// <summary>
/// Updates a monitored service.
/// </summary>
public sealed class UpdateService
{
    private readonly IServiceRepository _serviceRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateService"/> class.
    /// </summary>
    /// <param name="serviceRepository">Repository used to update the service.</param>
    public UpdateService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    /// <summary>
    /// Updates an existing monitored service.
    /// </summary>
    /// <param name="id">Service identifier.</param>
    /// <param name="name">New display name.</param>
    /// <param name="endpoint">New endpoint.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated service.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when the service does not exist.
    /// </exception>
    public async Task<Service> ExecuteAsync(Guid id, string name, Uri endpoint, CancellationToken cancellationToken = default)
    {
        var service = await _serviceRepository.GetByIdAsync(id, cancellationToken);

        if (service is null)
            throw new KeyNotFoundException($"Service '{id}' was not found.");

        service.Name = name;
        service.Endpoint = endpoint;

        await _serviceRepository.UpdateAsync(service, cancellationToken);

        return service;
    }
}