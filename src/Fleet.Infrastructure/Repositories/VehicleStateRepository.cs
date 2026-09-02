using Fleet.Application.Interfaces;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Infrastructure.Repositories;

/// <summary>
/// Implementa las operaciones de persistencia relacionadas
/// con el estado actual de los vehículos.
/// </summary>
public sealed class VehicleStateRepository : IVehicleStateRepository
{
    private readonly FleetDbContext _dbContext;

    public VehicleStateRepository(FleetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene el estado actual de un vehículo.
    /// </summary>
    public async Task<VehicleState?> GetByVehicleIdAsync(
        string vehicleId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.VehicleStates
            .FirstOrDefaultAsync(
                state => state.VehicleId == vehicleId,
                cancellationToken);
    }

    /// <summary>
    /// Inserta o actualiza el estado actual de un vehículo.
    /// </summary>
    public async Task UpsertAsync(
        VehicleState vehicleState,
        CancellationToken cancellationToken = default)
    {
        var existingState = await _dbContext.VehicleStates
            .FirstOrDefaultAsync(
                state => state.VehicleId == vehicleState.VehicleId,
                cancellationToken);

        if (existingState is null)
        {
            await _dbContext.VehicleStates.AddAsync(
                vehicleState,
                cancellationToken);
        }
        else
        {
            existingState.Update(
                vehicleState.Latitude,
                vehicleState.Longitude,
                vehicleState.Speed,
                vehicleState.LastTelemetryAt);
        }        
    }
}