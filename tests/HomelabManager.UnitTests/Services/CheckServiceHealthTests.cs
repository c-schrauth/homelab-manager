using HomelabManager.Application.Services;
using HomelabManager.Core.Health;
using HomelabManager.Core.Models;

namespace HomelabManager.UnitTests.Services;

/// <summary>
/// Tests for <see cref="CheckServiceHealth"/>.
/// </summary>
public class CheckServiceHealthTests
{
    [Fact]
    public async Task ExecuteAsync_WhenHealthCheckSucceeds_ShouldUpdateServiceStatus()
    {
        var service = CreateService();
        var checker = new FakeHealthChecker(HealthCheckResult.Healthy(TimeSpan.FromMilliseconds(100)));
        var repository = new FakeServiceRepository(service);
        var useCase = new CheckServiceHealth(repository, checker);

        var result = await useCase.ExecuteAsync(service.Id);

        Assert.Equal(ServiceStatus.Healthy, result.Status);
        Assert.Equal(ServiceStatus.Healthy, service.Status);
    }

    [Fact]
    public async Task ExecuteAsync_WhenHealthCheckFails_ShouldUpdateServiceStatus()
    {
        var service = CreateService();
        var checker = new FakeHealthChecker(HealthCheckResult.Unhealthy(TimeSpan.FromMilliseconds(100), "Connection failed"));
        var repository = new FakeServiceRepository(service);
        var useCase = new CheckServiceHealth(repository, checker);

        var result = await useCase.ExecuteAsync(service.Id);

        Assert.Equal(ServiceStatus.Unhealthy, result.Status);
        Assert.Equal(ServiceStatus.Unhealthy, service.Status);
        Assert.Equal("Connection failed", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_WhenServiceDoesNotExist_ShouldThrow()
    {
        var checker = new FakeHealthChecker(HealthCheckResult.Healthy(TimeSpan.FromMilliseconds(100)));
        var repository = new FakeServiceRepository();
        var useCase = new CheckServiceHealth(repository, checker);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.ExecuteAsync(Guid.NewGuid()));
    }

    private static Service CreateService()
    {
        return new Service
        {
            Id = Guid.NewGuid(),
            Name = "Test Service",
            Endpoint = new Uri("https://example.test")
        };
    }

    private sealed class FakeServiceRepository : IServiceRepository
    {
        private readonly Service? _service;

        public FakeServiceRepository(Service? service = null)
        {
            _service = service;
        }

        public Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (_service?.Id == id)
                return Task.FromResult<Service?>(_service);
            
            return Task.FromResult<Service?>(null);
        }
    }

    private sealed class FakeHealthChecker : IHealthChecker
    {
        private readonly HealthCheckResult _result;

        public FakeHealthChecker(HealthCheckResult result)
        {
            _result = result;
        }

        public Task<HealthCheckResult> CheckAsync(Uri endpoint, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_result);
        }
    }
}