using HomelabManager.Core.Health;
using HomelabManager.Core.Models;

namespace HomelabManager.IntegrationTests.TestDoubles;

public sealed class FakeHealthChecker : IHealthChecker
{
    public Task<HealthCheckResult> CheckAsync(Uri endpoint, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new HealthCheckResult(ServiceStatus.Healthy, TimeSpan.FromMilliseconds(10), null));
    }
}