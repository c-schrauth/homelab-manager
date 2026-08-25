using System.Net;
using System.Net.Http.Json;
using HomelabManager.Api.Models;
using HomelabManager.Core.Models;

namespace HomelabManager.IntegrationTests;

public sealed class ServiceHealthApiTests : IClassFixture<TestApplicationFactory>
{
    private readonly TestApplicationFactory _factory;
    private readonly HttpClient _client;

    public ServiceHealthApiTests(TestApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task HealthCheck_ReturnsHealthy_WhenServiceExists()
    {
        var service = new Service
        {
            Id = Guid.NewGuid(),
            Name = "Test Service",
            Endpoint = new Uri("https://test.example")
        };
        _factory.ServiceRepository.Add(service);

        var response = await _client.GetAsync($"/api/services/{service.Id}/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<HealthCheckResponse>();
        Assert.NotNull(result);
        Assert.Equal("Healthy", result.Status);
    }

    private sealed record HealthCheckResponse(string Status, TimeSpan Duration, string? ErrorMessage);
}