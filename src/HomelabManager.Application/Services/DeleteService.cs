using HomelabManager.Core.Models;

namespace HomelabManager.Application.Services;

/// <summary>
/// Deletes a monitored service.
/// </summary>
public sealed class DeleteService
{
    private readonly IServiceRepository _serviceRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteService"/> class.
    /// </summary>
    /// <param name="serviceRepository">Repository used to delete the service.</param>
    public DeleteService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    /// <summary>
    /// Deletes an existing monitored service.
    /// </summary>
    /// <param name="id">Service identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when the service does not exist.
    /// </exception>
    public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var deleted = await _serviceRepository.DeleteAsync(id, cancellationToken);

        if (!deleted)
            throw new KeyNotFoundException($"Service '{id}' was not found.");
    }
}