namespace Fleet.Domain.Entities;

/// <summary>
/// Representa un evento de telemetría generado por un vehículo de la flota.
/// </summary>
public class Telemetry
{
    /// <summary>
    /// Obtiene el identificador único del evento de telemetría.
    /// Se utiliza para garantizar el procesamiento idempotente de los eventos.
    /// </summary>
    public Guid EventId { get; private set; }

    /// <summary>
    /// Obtiene el identificador de negocio del vehículo que generó el evento.
    /// </summary>
    public string VehicleId { get; private set; } = string.Empty;

    /// <summary>
    /// Obtiene la latitud geográfica donde se generó el evento.
    /// </summary>
    public double Latitude { get; private set; }

    /// <summary>
    /// Obtiene la longitud geográfica donde se generó el evento.
    /// </summary>
    public double Longitude { get; private set; }

    /// <summary>
    /// Obtiene la velocidad del vehículo al momento de generar el evento.
    /// </summary>
    public double Speed { get; private set; }

    /// <summary>
    /// Obtiene la fecha y hora en que se generó el evento de telemetría.
    /// </summary>
    public DateTime Timestamp { get; private set; }

    /// <summary>
    /// Constructor requerido por Entity Framework Core.
    /// </summary>
    private Telemetry()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de un evento de telemetría.
    /// </summary>
    /// <param name="eventId">Identificador único del evento.</param>
    /// <param name="vehicleId">Identificador de negocio del vehículo.</param>
    /// <param name="latitude">Latitud geográfica.</param>
    /// <param name="longitude">Longitud geográfica.</param>
    /// <param name="speed">Velocidad del vehículo.</param>
    /// <param name="timestamp">Fecha y hora en que se generó el evento.</param>
    public Telemetry(
        Guid eventId,
        string vehicleId,
        double latitude,
        double longitude,
        double speed,
        DateTime timestamp)
    {
        if (eventId == Guid.Empty)
            throw new ArgumentException(
                "El identificador del evento es obligatorio.",
                nameof(eventId));

        if (string.IsNullOrWhiteSpace(vehicleId))
            throw new ArgumentException(
                "El identificador del vehículo es obligatorio.",
                nameof(vehicleId));

        if (latitude is < -90 or > 90)
            throw new ArgumentOutOfRangeException(
                nameof(latitude),
                "La latitud debe estar entre -90 y 90.");

        if (longitude is < -180 or > 180)
            throw new ArgumentOutOfRangeException(
                nameof(longitude),
                "La longitud debe estar entre -180 y 180.");

        if (speed < 0)
            throw new ArgumentOutOfRangeException(
                nameof(speed),
                "La velocidad no puede ser negativa.");

        EventId = eventId;
        VehicleId = vehicleId;
        Latitude = latitude;
        Longitude = longitude;
        Speed = speed;
        Timestamp = timestamp;
    }
}