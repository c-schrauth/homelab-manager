using HomelabManager.Application.Services;
using HomelabManager.Core.Models;

namespace HomelabManager.Infrastructure.Services;

/// <summary>
/// In-memory implementation of the service repository.
/// </summary>
public sealed class InMemoryServiceRepository : IServiceRepository
{
    private readonly List<Service> _services =
    [
        new Service
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Example Service",
            Endpoint = new Uri("https://example.test")
        }
    ];

    /// <inheritdoc />
    public Task<IReadOnlyList<Service>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Service> services = _services.ToList();

        return Task.FromResult(services);
    }

    /// <inheritdoc />
    public Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var service = _services.FirstOrDefault(service => service.Id == id);

        return Task.FromResult(service);
    }

    /// <inheritdoc />
    public Task AddAsync(Service service, CancellationToken cancellationToken = default)
    {
        _services.Add(service);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task UpdateAsync(Service service, CancellationToken cancellationToken = default)
    {
        var index = _services.FindIndex(existing => existing.Id == service.Id);

        if (index >= 0)
            _services[index] = service;
        
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var service = _services.FirstOrDefault(service => service.Id == id);

        if (service is null)
            return Task.FromResult(false);
        
        _services.Remove(service);

        return Task.FromResult(true);
    }
}