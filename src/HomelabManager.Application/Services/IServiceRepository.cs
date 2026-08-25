using HomelabManager.Core.Models;

namespace HomelabManager.Application.Services;

/// <summary>
/// Provides access to monitored services.
/// </summary>
public interface IServiceRepository
{
    /// <summary>
    /// Gets a service by its identifier.
    /// </summary>
    /// <param name="id">Service identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The servie, or <see langword="null"/> if it does not exist.</returns>
    Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}