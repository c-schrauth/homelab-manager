using HomelabManager.Application.Services;
using HomelabManager.Core.Models;

namespace HomelabManager.UnitTests.Services;

public sealed class UpdateServiceTests
{
    [Fact]
    public async Task ExecuteAsync_UpdatesExistingService()
    {
        var service = new Service
        {
            Id = Guid.NewGuid(),
            Name = "Original Service",
            Endpoint = new Uri("https://original.example")
        };

        var repository = new FakeServiceRepository(service);
        var useCase = new UpdateService(repository);

        var result = await useCase.ExecuteAsync(service.Id, "Updated Service", new Uri("https://updated.example"));

        Assert.Equal(service.Id, result.Id);
        Assert.Equal("Updated Service", result.Name);
        Assert.Equal(new Uri("https://updated.example"), result.Endpoint);
        Assert.Same(service, repository.UpdatedService);
    }

    [Fact]
    public async Task ExecuteAsync_ThrowsKeyNotFoundException_WhenServiceDoesNotExist()
    {
        var repository = new FakeServiceRepository(null);
        var useCase = new UpdateService(repository);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.ExecuteAsync(Guid.NewGuid(), "Updated Service", new Uri("https://updated.example")));
    }

    private sealed class FakeServiceRepository : IServiceRepository
    {
        private readonly Service? _service;

        public Service? UpdatedService { get; private set; }

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
            UpdatedService = service;

            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }
    }
}