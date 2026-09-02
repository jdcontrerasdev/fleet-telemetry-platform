namespace Fleet.Application.Interfaces;

/// <summary>
/// Define las operaciones necesarias para controlar
/// la idempotencia de los eventos de telemetría.
/// </summary>
public interface IProcessedTelemetryEventRepository
{
    /// <summary>
    /// Registra un evento como procesado si todavía no existe.
    /// </summary>
    /// <param name="eventId">
    /// Identificador único del evento.
    /// </param>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar la operación.
    /// </param>
    /// <returns>
    /// True si el evento fue registrado por primera vez;
    /// false si ya había sido registrado anteriormente.
    /// </returns>
    Task<bool> TryRegisterAsync(
        Guid eventId,
        CancellationToken cancellationToken = default);
}