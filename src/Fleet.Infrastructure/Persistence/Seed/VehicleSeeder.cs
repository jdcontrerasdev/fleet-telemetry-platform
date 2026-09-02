using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Infrastructure.Persistence.Seed;

/// <summary>
/// Crea los vehículos iniciales requeridos para el entorno de desarrollo.
/// </summary>
public static class VehicleSeeder
{
    public static async Task SeedAsync(
        FleetDbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        if (await dbContext.Vehicles.AnyAsync(cancellationToken))
        {
            return;
        }

        var vehicles = new[]
        {
            new Vehicle("TRUCK-001", "Camión 001"),
            new Vehicle("TRUCK-002", "Camión 002"),
            new Vehicle("TRUCK-003", "Camión 003"),
            new Vehicle("VAN-001", "Van 001"),
            new Vehicle("VAN-002", "Van 002")
        };

        await dbContext.Vehicles.AddRangeAsync(
            vehicles,
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}