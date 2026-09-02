namespace Fleet.Domain.Entities;

/// <summary>
/// Representa el último estado conocido de un vehículo.
/// </summary>
public class VehicleState
{
    /// <summary>
    /// Obtiene el identificador de negocio del vehículo.
    /// </summary>
    public string VehicleId { get; private set; } = string.Empty;

    /// <summary>
    /// Obtiene la última latitud conocida del vehículo.
    /// </summary>
    public double Latitude { get; private set; }

    /// <summary>
    /// Obtiene la última longitud conocida del vehículo.
    /// </summary>
    public double Longitude { get; private set; }

    /// <summary>
    /// Obtiene la última velocidad conocida del vehículo.
    /// </summary>
    public double Speed { get; private set; }

    /// <summary>
    /// Obtiene la fecha y hora de la última telemetría procesada.
    /// </summary>
    public DateTime LastTelemetryAt { get; private set; }

    /// <summary>
    /// Constructor requerido por Entity Framework Core.
    /// </summary>
    private VehicleState()
    {
    }

    /// <summary>
    /// Crea el estado actual de un vehículo.
    /// </summary>
    /// <param name="vehicleId">Identificador de negocio del vehículo.</param>
    /// <param name="latitude">Última latitud conocida.</param>
    /// <param name="longitude">Última longitud conocida.</param>
    /// <param name="speed">Última velocidad conocida.</param>
    /// <param name="lastTelemetryAt">Fecha y hora de la última telemetría.</param>
    public VehicleState(
        string vehicleId,
        double latitude,
        double longitude,
        double speed,
        DateTime lastTelemetryAt)
    {
        if (string.IsNullOrWhiteSpace(vehicleId))
            throw new ArgumentException(
                "El identificador del vehículo es obligatorio.",
                nameof(vehicleId));

        VehicleId = vehicleId;
        Latitude = latitude;
        Longitude = longitude;
        Speed = speed;
        LastTelemetryAt = lastTelemetryAt;
    }

    /// <summary>
    /// Actualiza el estado actual del vehículo con una nueva lectura de telemetría.
    /// </summary>
    /// <param name="latitude">Nueva latitud.</param>
    /// <param name="longitude">Nueva longitud.</param>
    /// <param name="speed">Nueva velocidad.</param>
    /// <param name="telemetryAt">Fecha y hora de la telemetría.</param>
    public void Update(
        double latitude,
        double longitude,
        double speed,
        DateTime telemetryAt)
    {
        Latitude = latitude;
        Longitude = longitude;
        Speed = speed;
        LastTelemetryAt = telemetryAt;
    }
}