using Fleet.Domain.Entities;
using Fleet.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Infrastructure.Persistence;

public class FleetDbContext : DbContext, IUnitOfWork
{
    public FleetDbContext(DbContextOptions<FleetDbContext> options)
        : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Telemetry> Telemetries => Set<Telemetry>();
    public DbSet<VehicleState> VehicleStates => Set<VehicleState>();
    public DbSet<Alert> Alerts => Set<Alert>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(FleetDbContext).Assembly);
    }

    Task IUnitOfWork.SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}