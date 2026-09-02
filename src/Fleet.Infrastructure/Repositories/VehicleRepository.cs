using Fleet.Application.Interfaces;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Infrastructure.Repositories;

/// <summary>
/// Implementa las operaciones de consulta de vehículos.
/// </summary>
public sealed class VehicleRepository : IVehicleRepository
{
    private readonly FleetDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="VehicleRepository"/>.
    /// </summary>
    /// <param name="dbContext">
    /// Contexto de persistencia de la aplicación.
    /// </param>
    public VehicleRepository(FleetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<Vehicle>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Vehicles
            .AsNoTracking()
            .OrderBy(vehicle => vehicle.VehicleId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Vehicle?> GetByVehicleIdAsync(
        string vehicleId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Vehicles
            .AsNoTracking()
            .FirstOrDefaultAsync(
                vehicle => vehicle.VehicleId == vehicleId,
                cancellationToken);
    }
}