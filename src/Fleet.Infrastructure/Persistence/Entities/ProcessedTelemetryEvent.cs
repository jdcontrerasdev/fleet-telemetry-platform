namespace Fleet.Infrastructure.Persistence.Entities;

/// <summary>
/// Representa el registro de un evento de telemetría que ya fue procesado.
/// Se utiliza para garantizar la idempotencia del procesamiento.
/// </summary>
public sealed class ProcessedTelemetryEvent
{
    /// <summary>
    /// Identificador único del evento de telemetría.
    /// </summary>
    public Guid EventId { get; private set; }

    /// <summary>
    /// Fecha y hora en la que el evento fue procesado.
    /// </summary>
    public DateTime ProcessedAt { get; private set; }

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="ProcessedTelemetryEvent"/>.
    /// </summary>
    /// <param name="eventId">
    /// Identificador único del evento procesado.
    /// </param>
    public ProcessedTelemetryEvent(Guid eventId)
    {
        if (eventId == Guid.Empty)
        {
            throw new ArgumentException(
                "El identificador del evento es obligatorio.",
                nameof(eventId));
        }

        EventId = eventId;
        ProcessedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Constructor utilizado por Entity Framework Core.
    /// </summary>
    private ProcessedTelemetryEvent()
    {
    }
}