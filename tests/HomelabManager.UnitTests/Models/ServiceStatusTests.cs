using HomelabManager.Core.Models;

namespace HomelabManager.UnitTests.Models;

/// <summary>
/// Tests for <see cref="ServiceStatus"/>.
/// </summary>
public class ServiceStatusTests
{
    [Fact]
    public void ServiceStatus_ShouldContainExpectedStates()
    {
        Assert.Equal(0, (int)ServiceStatus.Unknown);
	Assert.Equal(1, (int)ServiceStatus.Healthy);
	Assert.Equal(2, (int)ServiceStatus.Unhealthy);
    }
}

