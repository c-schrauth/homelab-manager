using HomelabManager.Application.Services;
using HomelabManager.Core.Models;

namespace HomelabManager.UnitTests.Services;

public sealed class GetServicesTests
{
    [Fact]
    public async Task ExecuteAsync_ReturnsAllServices()
    {
        var service = new Service
        {
            Id = Guid.NewGuid(),
            Name = "Test Service",
            Endpoint = new Uri("https://test.example")
        };

        var repository = new FakeServiceRepository(service);
        var useCase = new GetServices(repository);

        var result = await useCase.ExecuteAsync();

        var returnedService = Assert.Single(result);
        Assert.Equal(service.Id, returnedService.Id);
    }

    private sealed class FakeServiceRepository : IServiceRepository
    {
        private readonly Service _service;

        public FakeServiceRepository(Service service)
        {
            _service = service;
        }

        public Task<IReadOnlyList<Service>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<Service> services = [_service];

            return Task.FromResult(services);
        }

        public Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<Service?>(null);
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