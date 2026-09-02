using Fleet.Domain.Enums;

namespace Fleet.Application.DTOs;

/// <summary>
/// Representa la información de un vehículo expuesta por la aplicación.
/// </summary>
public sealed record VehicleDto(
    Guid Id,
    string VehicleId,
    string Name,
    VehicleStatus Status);