using HomelabManager.Core.Models;
using HomelabManager.Application.Services;
using HomelabManager.Infrastructure.Services;

namespace HomelabManager.Infrastructure.Services;

/// <summary>
/// In-memory implementation of the service repository.
/// </summary>
public sealed class InMemoryServiceRepository : IServiceRepository
{
    private readonly List<Service> _services =
    [
        new Service
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Example Service",
            Endpoint = new Uri("https://example.test")
        }
    ];

    /// <inheritdoc />
    public Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var service = _services.FirstOrDefault(service => service.Id == id);

        return Task.FromResult(service);
    }
}