using HomelabManager.Application.Services;
using HomelabManager.Core.Models;

namespace HomelabManager.IntegrationTests.TestDoubles;

public sealed class FakeServiceRepository : IServiceRepository
{
    private readonly Dictionary<Guid, Service> _services = new();

    public void Add(Service service)
    {
        _services[service.Id] = service;
    }

    public Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _services.TryGetValue(id, out var service);

        return Task.FromResult(service);
    }
}