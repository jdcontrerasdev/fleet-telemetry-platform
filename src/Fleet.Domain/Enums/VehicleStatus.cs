namespace Fleet.Domain.Enums;

/// <summary>
/// Representa el estado operativo actual de un vehículo.
/// </summary>
public enum VehicleStatus
{
    /// <summary>
    /// El vehículo está activo y operando.
    /// </summary>
    Active = 1,

    /// <summary>
    /// El vehículo se encuentra detenido.
    /// </summary>
    Stopped = 2,

    /// <summary>
    /// No se ha recibido telemetría reciente del vehículo.
    /// </summary>
    Offline = 3
}