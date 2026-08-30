using System.IO.Pipelines;
using HomelabManager.Core.Models;
using HomelabManager.Infrastructure.Services;

namespace HomelabManager.UnitTests.Infrastructure.Services;

public sealed class InMemoryServiceRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsAllServices()
    {
        var repository = new InMemoryServiceRepository();

        var services = await repository.GetAllAsync();

        Assert.Single(services);
        Assert.Equal("Example Service", services[0].Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsService_WhenServiceExists()
    {
        var repository = new InMemoryServiceRepository();

        var service = await repository.GetByIdAsync(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        Assert.NotNull(service);
        Assert.Equal("Example Service", service.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenServiceDoesNotExist()
    {
        var repository = new InMemoryServiceRepository();

        var service = await repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(service);
    }

    [Fact]
    public async Task AddAsync_AddsService()
    {
        var repository = new InMemoryServiceRepository();
        var service = new Service
        {
            Id = Guid.NewGuid(),
            Name = "Test Service",
            Endpoint = new Uri("https://test.example")
        };

        await repository.AddAsync(service);

        var result = await repository.GetByIdAsync(service.Id);

        Assert.NotNull(result);
        Assert.Equal("Test Service", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesExistingService()
    {
        var repository = new InMemoryServiceRepository();

        var service = await repository.GetByIdAsync(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        Assert.NotNull(service);

        service.Name = "Updated Service";

        await repository.UpdateAsync(service);

        var result = await repository.GetByIdAsync(service.Id);

        Assert.NotNull(result);
        Assert.Equal("Updated Service", result.Name);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenServiceExists()
    {
        var repository = new InMemoryServiceRepository();
        var id = Guid.Parse("11111111-1111-1111-1111-111111111111");

        var deleted = await repository.DeleteAsync(id);

        Assert.True(deleted);
        Assert.Null(await repository.GetByIdAsync(id));
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenServiceDoesNotExist()
    {
        var repository = new InMemoryServiceRepository();

        var deleted = await repository.DeleteAsync(Guid.NewGuid());

        Assert.False(deleted);
    }
}