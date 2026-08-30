using HomelabManager.Application.Services;
using HomelabManager.Core.Models;
using HomelabManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HomelabManager.Infrastructure.Services;

/// <summary>
/// Entity Framework Core implementation of the service repository.
/// </summary>
public sealed class ServiceRepository : IServiceRepository
{
    private readonly ServiceDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceRepository"/> class.
    /// </summary>
    /// <param name="dbContext">Database context.</param>
    public ServiceRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Service>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Services.AsNoTracking().ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Services.FirstOrDefaultAsync(service => service.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(Service service, CancellationToken cancellationToken = default)
    {
        await _dbContext.Services.AddAsync(service, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Service service, CancellationToken cancellationToken = default)
    {
        _dbContext.Services.Update(service);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var service = await _dbContext.Services.FirstOrDefaultAsync(service => service.Id == id, cancellationToken);
        if (service is null)
            return false;
        
        _dbContext.Services.Remove(service);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}