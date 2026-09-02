namespace Fleet.Application.DTOs;

/// <summary>
/// Representa la respuesta generada después de aceptar un evento de telemetría.
/// </summary>
public sealed record TelemetryResponse(
    Guid EventId,
    string Status);