using HomelabManager.Application.Services;
using HomelabManager.Core.Models;

namespace HomelabManager.UnitTests.Services;

public sealed class CreateServiceTests
{
    [Fact]
    public async Task ExecuteAsync_CreatesServiceWithGeneratedId()
    {
        var repository = new FakeServiceRepository();
        var useCase = new CreateService(repository);

        var result = await useCase.ExecuteAsync("Test Service", new Uri("https://test.example"));

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Test Service", result.Name);
        Assert.Equal(new Uri("https://test.example"), result.Endpoint);
        Assert.Equal(ServiceStatus.Unknown, result.Status);
        Assert.Same(result, repository.AddedService);
    }

    private sealed class FakeServiceRepository : IServiceRepository
    {
        public Service? AddedService { get; private set; }

        public Task<IReadOnlyList<Service>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<Service> services = AddedService is null ? [] : [AddedService];

            return Task.FromResult(services);
        }

        public Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<Service?>(null);
        }

        public Task AddAsync(Service service, CancellationToken cancellationToken = default)
        {
            AddedService = service;

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