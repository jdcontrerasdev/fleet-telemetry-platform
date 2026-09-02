using Fleet.Application.DTOs;
using Fleet.Application.Interfaces;

namespace Fleet.Application.Services;

/// <summary>
/// Proporciona operaciones de consulta relacionadas
/// con la telemetría de los vehículos.
/// </summary>
public sealed class TelemetryQueryService
{
    private readonly ITelemetryRepository _telemetryRepository;

    public TelemetryQueryService(
        ITelemetryRepository telemetryRepository)
    {
        _telemetryRepository = telemetryRepository;
    }

    /// <summary>
    /// Obtiene el histórico de telemetría de un vehículo.
    /// </summary>
    public async Task<IReadOnlyCollection<TelemetryDto>> GetByVehicleIdAsync(
        string vehicleId,
        CancellationToken cancellationToken = default)
    {
        var telemetry = await _telemetryRepository.GetByVehicleIdAsync(
            vehicleId,
            cancellationToken);

        return telemetry
            .Select(item => new TelemetryDto(
                item.EventId,
                item.VehicleId,
                item.Latitude,
                item.Longitude,
                item.Speed,
                item.Timestamp))
            .ToList();
    }
}