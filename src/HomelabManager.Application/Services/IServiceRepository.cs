using HomelabManager.Core.Models;

namespace HomelabManager.Application.Services;

/// <summary>
/// Provides access to monitored services.
/// </summary>
public interface IServiceRepository
{
    /// <summary>
    /// Gets all monitored services.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All monitored services.</returns>
    Task<IReadOnlyList<Service>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a service by its identifier.
    /// </summary>
    /// <param name="id">Service identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The servie, or <see langword="null"/> if it does not exist.</returns>
    Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new service.
    /// </summary>
    /// <param name="service">Service to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    Task AddAsync(Service service, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing service.
    /// </summary>
    /// <param name="service">Service to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    Task UpdateAsync(Service service, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a service.
    /// </summary>
    /// <param name="id">Service identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns><see langword="true"/> if the service existed and was deleted.</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}