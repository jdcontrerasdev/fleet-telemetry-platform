namespace Fleet.Application.DTOs;

/// <summary>
/// Representa información de telemetría expuesta por la API.
/// </summary>
public sealed record TelemetryDto(
    Guid EventId,
    string VehicleId,
    double Latitude,
    double Longitude,
    double Speed,
    DateTime Timestamp);