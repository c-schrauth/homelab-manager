using System.Net;
using System.Net.Http.Json;
using HomelabManager.Api.Controllers;
using HomelabManager.Api.Models;

namespace HomelabManager.IntegrationTests.Api;

public sealed class ServiceApiTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public ServiceApiTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateService_ReturnsCreatedService()
    {
        var request = new CreateServiceRequest
        {
            Name = "Integration Test Service",
            Endpoint = new Uri("https://integration-test.example")
        };

        var response = await _client.PostAsJsonAsync("/api/services", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var service = await response.Content.ReadFromJsonAsync<ServiceResponse>();

        Assert.NotNull(service);
        Assert.NotEqual(Guid.Empty, service.Id);
        Assert.Equal(request.Name, service.Name);
        Assert.Equal(request.Endpoint, service.Endpoint);
        Assert.Equal(Core.Models.ServiceStatus.Unknown, service.Status);

        Assert.NotNull(response.Headers.Location);
        Assert.Equal($"/api/services/{service.Id}", response.Headers.Location!.AbsolutePath);
    }

    [Fact]
    public async Task CreateService_CanBeRetrievedAfterCreation()
    {
        var request = new CreateServiceRequest
        {
            Name = "Retrievable Service",
            Endpoint = new Uri("https://retrievable.example")
        };

        var createResponse = await _client.PostAsJsonAsync("/api/services", request);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdService = await createResponse.Content.ReadFromJsonAsync<ServiceResponse>();

        Assert.NotNull(createdService);

        var getResponse = await _client.GetAsync($"/api/services/{createdService.Id}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var retrievedService = await getResponse.Content.ReadFromJsonAsync<ServiceResponse>();

        Assert.NotNull(retrievedService);
        Assert.Equal(createdService.Id, retrievedService.Id);
        Assert.Equal(request.Name, retrievedService.Name);
        Assert.Equal(request.Endpoint, retrievedService.Endpoint);
    }

    [Fact]
    public async Task UpdateService_ReturnsUpdatedService()
    {
        var createRequest = new CreateServiceRequest
        {
            Name = "Service Before Update",
            Endpoint = new Uri("https://before.example")
        };

        var createResponse = await _client.PostAsJsonAsync("/api/services", createRequest);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdService = await createResponse.Content.ReadFromJsonAsync<ServiceResponse>();

        Assert.NotNull(createdService);

        var updateRequest = new UpdateServiceRequest
        {
            Name = "Service After Update",
            Endpoint = new Uri("https://after.example")
        };

        var updateResponse = await _client.PutAsJsonAsync($"/api/services/{createdService.Id}", updateRequest);

        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updatedService = await updateResponse.Content.ReadFromJsonAsync<ServiceResponse>();

        Assert.NotNull(updatedService);
        Assert.Equal(createdService.Id, updatedService.Id);
        Assert.Equal(updateRequest.Name, updatedService.Name);
        Assert.Equal(updateRequest.Endpoint, updatedService.Endpoint);
    }

    [Fact]
    public async Task UpdateService_ReturnsNotFound_WhenServiceDoesNotExist()
    {
        var request = new UpdateServiceRequest
        {
            Name = "Non-existent Service",
            Endpoint = new Uri("https://does-not-exist.example")
        };

        var response = await _client.PutAsJsonAsync($"/api/services/{Guid.NewGuid()}", request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteService_ReturnsNoContent()
    {
        var createRequest = new CreateServiceRequest
        {
            Name = "Service To Delete",
            Endpoint = new Uri("https://delete.example")
        };

        var createResponse = await _client.PostAsJsonAsync("/api/services", createRequest);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdService = await createResponse.Content.ReadFromJsonAsync<ServiceResponse>();

        Assert.NotNull(createdService);

        var deleteResponse = await _client.DeleteAsync($"/api/services/{createdService.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/services/{createdService.Id}");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteService_ReturnsNotFound_WhenServiceDoesNotExist()
    {
        var response = await _client.DeleteAsync($"/api/services/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}