using Fleet.Domain.Entities;

namespace Fleet.Application.Interfaces;

/// <summary>
/// Define las operaciones de persistencia y consulta de telemetría.
/// </summary>
public interface ITelemetryRepository
{
    /// <summary>
    /// Verifica si un evento de telemetría ya fue procesado.
    /// </summary>
    /// <param name="eventId">Identificador único del evento.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>
    /// <c>true</c> si el evento ya existe; de lo contrario, <c>false</c>.
    /// </returns>
    Task<bool> ExistsAsync(
        Guid eventId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Persiste un evento de telemetría.
    /// </summary>
    /// <param name="telemetry">Evento de telemetría.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    Task AddAsync(
        Telemetry telemetry,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene la colección de registros de telemetría asociados
    /// </summary>
    /// <param name="vehicleId">Identificador único del vehículo.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    Task<IReadOnlyCollection<Telemetry>> GetByVehicleIdAsync(
    string vehicleId,
    CancellationToken cancellationToken = default);
}