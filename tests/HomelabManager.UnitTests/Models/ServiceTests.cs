using HomelabManager.Core.Models;

namespace HomelabManager.UnitTests.Models;

/// <summary>
/// Tests for <see cref="Service"/>.
/// </summary>
public class ServiceTests
{
    [Fact]
    public void NewService_ShouldHaveUnknownStatus()
    {
        var service = new Service
        {
            Id = Guid.NewGuid(),
            Name = "Home Assistant",
            Endpoint = new Uri("https://homeassistant.example.test")
        };

        Assert.Equal(ServiceStatus.Unknown, service.Status);
    }

    [Fact]
    public void Service_ShouldStoreConfiguration()
    {
        var id = Guid.NewGuid();
        var endpoint = new Uri("https://homeassistant.example.test");

        var service = new Service
        {
            Id = id,
            Name = "Home Assistant",
            Endpoint = endpoint
        };

        Assert.Equal(id, service.Id);
        Assert.Equal("Home Assistant", service.Name);
        Assert.Equal(endpoint, service.Endpoint);
    }
}