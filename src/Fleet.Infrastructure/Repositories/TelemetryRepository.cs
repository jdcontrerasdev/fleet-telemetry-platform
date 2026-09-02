using Fleet.Application.Interfaces;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Infrastructure.Repositories;

/// <summary>
/// Implementa las operaciones de persistencia relacionadas con la telemetría.
/// </summary>
public sealed class TelemetryRepository : ITelemetryRepository
{
    private readonly FleetDbContext _dbContext;

    public TelemetryRepository(FleetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Verifica si existe un evento de telemetría con el identificador indicado.
    /// </summary>
    public async Task<bool> ExistsAsync(
        Guid eventId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Telemetries
            .AnyAsync(
                telemetry => telemetry.EventId == eventId,
                cancellationToken);
    }

    /// <summary>
    /// Agrega un evento de telemetría al contexto de persistencia.
    /// </summary>
    public async Task AddAsync(
        Telemetry telemetry,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Telemetries.AddAsync(
            telemetry,
            cancellationToken);        
    }
}