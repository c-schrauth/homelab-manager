using HomelabManager.Application.Services;
using HomelabManager.Core.Models;

namespace HomelabManager.IntegrationTests.TestDoubles;

public sealed class FakeServiceRepository : IServiceRepository
{
    private readonly List<Service> _services = [];

    public Task<IReadOnlyList<Service>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Service> services = _services.ToList();

        return Task.FromResult(services);
    }

    public Task AddAsync(Service service, CancellationToken cancellationToken = default)
    {
        _services.Add(service);

        return Task.CompletedTask;
    }

    public Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var service = _services.FirstOrDefault(service => service.Id == id);

        return Task.FromResult(service);
    }

    public Task UpdateAsync(Service service, CancellationToken cancellationToken = default)
    {
        var index = _services.FindIndex(existing => existing.Id == service.Id);
        
        if (index >= 0)
            _services[index] = service;

        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var service = _services.FirstOrDefault(service => service.Id == id);

        if (service is null)
            return Task.FromResult(false);
        
        _services.Remove(service);

        return Task.FromResult(true);
    }
}