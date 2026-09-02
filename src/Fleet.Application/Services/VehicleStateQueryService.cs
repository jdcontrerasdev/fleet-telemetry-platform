using Fleet.Application.DTOs;
using Fleet.Application.Interfaces;

namespace Fleet.Application.Services;

/// <summary>
/// Proporciona operaciones de consulta relacionadas
/// con el estado actual de los vehículos.
/// </summary>
public sealed class VehicleStateQueryService
{
    private readonly IVehicleStateRepository _vehicleStateRepository;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="VehicleStateQueryService"/>.
    /// </summary>
    /// <param name="vehicleStateRepository">
    /// Repositorio utilizado para consultar el estado actual de los vehículos.
    /// </param>
    public VehicleStateQueryService(
        IVehicleStateRepository vehicleStateRepository)
    {
        _vehicleStateRepository = vehicleStateRepository;
    }

    /// <summary>
    /// Obtiene el estado actual de un vehículo.
    /// </summary>
    /// <param name="vehicleId">
    /// Identificador único del vehículo.
    /// </param>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar la operación asíncrona.
    /// </param>
    /// <returns>
    /// Estado actual del vehículo o <c>null</c> si no existe información.
    /// </returns>
    public async Task<VehicleStateDto?> GetByVehicleIdAsync(
        string vehicleId,
        CancellationToken cancellationToken = default)
    {
        var state = await _vehicleStateRepository.GetByVehicleIdAsync(
            vehicleId,
            cancellationToken);

        if (state is null)
        {
            return null;
        }

        return new VehicleStateDto(
            state.VehicleId,
            state.Latitude,
            state.Longitude,
            state.Speed,
            state.LastTelemetryAt);
    }
}