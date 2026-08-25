using HomelabManager.Core.Health;
using HomelabManager.Core.Models;

namespace HomelabManager.UnitTests.Health;

/// <summary>
/// Tests for <see cref="HealthCheckResult"/>. 
/// </summary>
public class HealthCheckResultsTests
{
    [Fact]
    public void Healthy_ShouldCreateHealthyResult()
    {
        var duration = TimeSpan.FromMilliseconds(125);
        
        var result = HealthCheckResult.Healthy(duration);

        Assert.Equal(ServiceStatus.Healthy, result.Status);
        Assert.Equal(duration, result.Duration);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public void Unhealthy_ShouldCreateUnhealthyResult()
    {
        var duration = TimeSpan.FromMilliseconds(500);

        var result = HealthCheckResult.Unhealthy(duration, "Connection refused");

        Assert.Equal(ServiceStatus.Unhealthy, result.Status);
        Assert.Equal(duration, result.Duration);
        Assert.Equal("Connection refused", result.ErrorMessage);
    }
}