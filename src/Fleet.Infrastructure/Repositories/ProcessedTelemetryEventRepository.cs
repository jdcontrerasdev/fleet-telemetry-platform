using Fleet.Application.Interfaces;
using Fleet.Infrastructure.Persistence;
using Fleet.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Infrastructure.Repositories;

/// <summary>
/// Implementa el control de idempotencia de eventos de telemetría.
/// </summary>
public sealed class ProcessedTelemetryEventRepository
    : IProcessedTelemetryEventRepository
{
    private readonly FleetDbContext _dbContext;

    public ProcessedTelemetryEventRepository(
        FleetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Registra un evento de telemetría como procesado de forma idempotente.
    /// </summary>
    /// <param name="eventId">
    /// Identificador único del evento de telemetría que se desea registrar.
    /// </param>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar la operación asíncrona.
    /// </param>
    /// <returns>
    /// <c>true</c> si el evento fue registrado correctamente por primera vez;
    /// <c>false</c> si el evento ya había sido registrado previamente.
    /// </returns>
    /// <remarks>
    /// La operación utiliza la restricción de unicidad de <c>EventId</c>
    /// en PostgreSQL junto con <c>ON CONFLICT DO NOTHING</c>.
    /// De esta forma, múltiples intentos concurrentes de registrar el mismo
    /// evento no generan registros duplicados.
    /// </remarks>
    public async Task<bool> TryRegisterAsync(
    Guid eventId,
    CancellationToken cancellationToken = default)
    {
        var processedAt = DateTime.UtcNow;

        var affectedRows = await _dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"""
            INSERT INTO processed_telemetry_events ("EventId", "ProcessedAt")
            VALUES ({eventId}, {processedAt})
            ON CONFLICT ("EventId") DO NOTHING
            """,
            cancellationToken);

        return affectedRows > 0;
    }
}