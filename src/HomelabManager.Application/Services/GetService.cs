using HomelabManager.Core.Models;

namespace HomelabManager.Application.Services;

/// <summary>
/// Retrieves a monitored service by its identifier.
/// </summary>
public sealed class GetService
{
    private readonly IServiceRepository _serviceRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetService"/> class.
    /// </summary>
    /// <param name="serviceRepository">Repository used to retrieve the service.</param>
    public GetService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    /// <summary>
    /// Retrieves a monitored service by its identifier.
    /// </summary>
    /// <param name="id">Service identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The service, or <see langword="null"/> if it does not exist.</returns>
    public Task<Service?> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _serviceRepository.GetByIdAsync(id, cancellationToken);
    }
}