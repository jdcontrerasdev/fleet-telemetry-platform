namespace Fleet.Application.DTOs;

/// <summary>
/// Representa la información recibida desde un vehículo.
/// </summary>
public sealed record TelemetryRequest(
    Guid EventId,
    string VehicleId,
    double Latitude,
    double Longitude,
    double Speed,
    DateTime Timestamp);