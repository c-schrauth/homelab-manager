using HomelabManager.Application.Services;
using HomelabManager.Core.Models;
using HomelabManager.Infrastructure.Persistence;
using HomelabManager.Infrastructure.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HomelabManager.UnitTests.Infrastructure.Services;

public sealed class ServiceRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<ServiceDbContext> _options;

    public ServiceRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<ServiceDbContext>().UseSqlite(_connection).Options;

        using var context = new ServiceDbContext(_options);
        context.Database.EnsureCreated();
    }

    [Fact]
    public async Task AddAsync_PersistsService()
    {
        var service = CreateService();

        await using (var context = new ServiceDbContext(_options))
        {
            var repository = new ServiceRepository(context);

            await repository.AddAsync(service);
        }

        await using (var context = new ServiceDbContext(_options))
        {
            var repository = new ServiceRepository(context);

            var result = await repository.GetByIdAsync(service.Id);

            Assert.NotNull(result);
            Assert.Equal(service.Name, result.Name);
            Assert.Equal(service.Endpoint, result.Endpoint);
            Assert.Equal(service.Status, result.Status);
        }
    }

    [Fact]
    public async Task GetAllAsync_ReturnsPersistedServices()
    {
        var first = CreateService("Service One");
        var second = CreateService("Service Two");

        await using (var context = new ServiceDbContext(_options))
        {
            var repository = new ServiceRepository(context);

            await repository.AddAsync(first);
            await repository.AddAsync(second);
        }

        await using (var context = new ServiceDbContext(_options))
        {
            var repository = new ServiceRepository(context);

            var result = await repository.GetAllAsync();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, service => service.Id == first.Id);
            Assert.Contains(result, service => service.Id == second.Id);
        }
    }

    [Fact]
    public async Task UpdateAsync_PersistsChanges()
    {
        var service = CreateService();

        await using (var context = new ServiceDbContext(_options))
        {
            var repository = new ServiceRepository(context);
            await repository.AddAsync(service);
        }

        service.Name = "Updated Service";
        service.Endpoint = new Uri("https://updated.example");

        await using (var context = new ServiceDbContext(_options))
        {
            var repository = new ServiceRepository(context);
            await repository.UpdateAsync(service);
        }

        await using (var context = new ServiceDbContext(_options))
        {
            var repository = new ServiceRepository(context);

            var result = await repository.GetByIdAsync(service.Id);

            Assert.NotNull(result);
            Assert.Equal("Updated Service", result.Name);
            Assert.Equal(new Uri("https://updated.example"), result.Endpoint);
        }
    }

    [Fact]
    public async Task DeleteAsync_RemovesService()
    {
        var service = CreateService();

        await using (var context = new ServiceDbContext(_options))
        {
            var repository = new ServiceRepository(context);
            await repository.AddAsync(service);
        }

        await using (var context = new ServiceDbContext(_options))
        {
            var repository = new ServiceRepository(context);

            var deleted = await repository.DeleteAsync(service.Id);

            Assert.True(deleted);
        }

        await using (var context = new ServiceDbContext(_options))
        {
            var repository = new ServiceRepository(context);

            var result = await repository.GetByIdAsync(service.Id);

            Assert.Null(result);
        }
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenServiceDoesNotExist()
    {
        await using var context = new ServiceDbContext(_options);
        var repository = new ServiceRepository(context);

        var deleted = await repository.DeleteAsync(Guid.NewGuid());

        Assert.False(deleted);
    }

    private static Service CreateService(string name = "Test Service")
    {
        return new Service
        {
            Id = Guid.NewGuid(),
            Name = name,
            Endpoint = new Uri("https://example.test"),
            Status = ServiceStatus.Unknown
        };
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}