using HomelabManager.Core.Models;

namespace HomelabManager.Application.Services;

/// <summary>
/// Retrieves all monitored services.
/// </summary>
public sealed class GetServices
{
    private readonly IServiceRepository _serviceRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetServices"/> class.
    /// </summary>
    /// <param name="serviceRepository">Repository used to retrieve services.</param>
    public GetServices(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    /// <summary>
    /// Retrieves all monitored services.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All monitored services.</returns>
    public Task<IReadOnlyList<Service>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return _serviceRepository.GetAllAsync(cancellationToken);
    }
}