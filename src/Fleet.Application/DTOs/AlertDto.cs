using Fleet.Domain.Enums;

namespace Fleet.Application.DTOs;

/// <summary>
/// Representa una alerta expuesta por la API.
/// </summary>
public sealed record AlertDto(
    Guid Id,
    string VehicleId,
    string Message,
    AlertSeverity Severity,
    DateTime CreatedAt,
    bool IsResolved);