using Fleet.Domain.Entities;

namespace Fleet.Application.Interfaces;

/// <summary>
/// Define el contrato para procesar eventos de telemetría
/// recibidos desde el sistema de mensajería.
/// </summary>
public interface ITelemetryProcessor
{
    /// <summary>
    /// Procesa un evento de telemetría y ejecuta las operaciones
    /// necesarias para actualizar la información del vehículo.
    /// </summary>
    /// <param name="telemetry">
    /// Evento de telemetría que será procesado.
    /// </param>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar la operación.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona.
    /// </returns>
    Task ProcessAsync(
        Telemetry telemetry,
        CancellationToken cancellationToken = default);
}