using System.Reflection;
using HomelabManager.Application.Services;
using HomelabManager.Core.Models;

namespace HomelabManager.UnitTests.Services;

public sealed class GetServiceTests
{
    [Fact]
    public async Task ExecuteAsync_ReturnsService_WhenServiceExists()
    {
        var service = new Service
        {
            Id = Guid.NewGuid(),
            Name = "Test Service",
            Endpoint = new Uri("https://test.example")
        };

        var repository = new FakeServiceRepository(service);
        var useCase = new GetService(repository);

        var result = await useCase.ExecuteAsync(service.Id);

        Assert.NotNull(result);
        Assert.Equal(service.Id, result.Id);
        Assert.Equal(service.Name, result.Name);
        Assert.Equal(service.Endpoint, result.Endpoint);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsNull_WhenServiceDoesNotExist()
    {
        var repository = new FakeServiceRepository(null);
        var useCase = new GetService(repository);

        var result = await useCase.ExecuteAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    private sealed class FakeServiceRepository : IServiceRepository
    {
        private readonly Service? _service;

        public FakeServiceRepository(Service? service)
        {
            _service = service;
        }

        public Task<IReadOnlyList<Service>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<Service> services = _service is null ? [] : [_service];

            return Task.FromResult(services);
        }

        public Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_service?.Id == id ? _service : null);
        }

        public Task AddAsync(Service service, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Service service, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }
    }
}