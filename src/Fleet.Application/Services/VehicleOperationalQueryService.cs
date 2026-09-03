using Fleet.Application.Interfaces;
using Fleet.Domain.Entities;

namespace Fleet.Application.Services;

/// <summary>
/// Ejecuta consultas operativas sobre el estado de los vehículos.
/// </summary>
public sealed class VehicleOperationalQueryService
{
    private readonly IVehicleStateRepository _vehicleStateRepository;

    public VehicleOperationalQueryService(
        IVehicleStateRepository vehicleStateRepository)
    {
        _vehicleStateRepository = vehicleStateRepository;
    }

    /// <summary>
    /// Obtiene los vehículos que permanecen detenidos
    /// durante más tiempo del indicado.
    /// </summary>
    public async Task<IReadOnlyCollection<VehicleState>> GetStoppedVehiclesAsync(
        int minutes,
        CancellationToken cancellationToken = default)
    {
        if (minutes <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(minutes),
                "Los minutos deben ser mayores que cero.");
        }

        var states = await _vehicleStateRepository.GetAllAsync(
            cancellationToken);

        var threshold = DateTime.UtcNow.AddMinutes(-minutes);

        return states
            .Where(state =>
                state.Speed == 0 &&
                state.LastTelemetryAt <= threshold)
            .OrderBy(state => state.LastTelemetryAt)
            .ToList();
    }
}