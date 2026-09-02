namespace Fleet.Application.DTOs;

/// <summary>
/// Representa el estado actual de un vehículo.
/// </summary>
public sealed record VehicleStateDto(
    string VehicleId,
    double Latitude,
    double Longitude,
    double Speed,
    DateTime LastTelemetryAt);