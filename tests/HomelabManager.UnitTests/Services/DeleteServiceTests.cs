using HomelabManager.Application.Services;
using HomelabManager.Core.Models;

namespace HomelabManager.UnitTests.Services;

public sealed class DeleteServiceTests
{
    [Fact]
    public async Task ExecuteAsync_DeletesExistingService()
    {
        var service = new Service
        {
            Id = Guid.NewGuid(),
            Name = "Test Service",
            Endpoint = new Uri("https://test.example")
        };

        var repository = new FakeServiceRepository(service);
        var useCase = new DeleteService(repository);

        await useCase.ExecuteAsync(service.Id);

        Assert.Equal(service.Id, repository.DeletedServiceId);
    }

    [Fact]
    public async Task ExecuteAsync_ThrowsKeyNotFoundException_WhenServiceDoesNotExist()
    {
        var repository = new FakeServiceRepository(null);
        var useCase = new DeleteService(repository);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.ExecuteAsync(Guid.NewGuid()));
    }

    private sealed class FakeServiceRepository : IServiceRepository
    {
        private readonly Service? _service;

        public Guid? DeletedServiceId { get; private set; }

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
            if (_service?.Id != id)
                return Task.FromResult(false);
            
            DeletedServiceId = id;

            return Task.FromResult(true);
        }
    }
}