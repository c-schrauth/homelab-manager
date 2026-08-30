using HomelabManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HomelabManager.Infrastructure.Persistence;

/// <summary>
/// Entity Framework core database context for Homelab Manager.
/// </summary>
public sealed class ServiceDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceDbContext"/> class.
    /// </summary>
    /// <param name="options">Database context options.</param>
    public ServiceDbContext(DbContextOptions<ServiceDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets the monitored services.
    /// </summary>
    public DbSet<Service> Services => Set<Service>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(service => service.Id);
            entity.Property(service => service.Name).IsRequired();
            entity.Property(service => service.Endpoint).IsRequired();
            entity.Property(service => service.Status).IsRequired();
        });
    }
}