using Fleet.Application.DTOs;
using Fleet.Application.Interfaces;

namespace Fleet.Application.Services;

/// <summary>
/// Proporciona las operaciones de consulta relacionadas
/// con los vehículos de la flota.
/// </summary>
public sealed class VehicleQueryService
{
    private readonly IVehicleRepository _vehicleRepository;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="VehicleQueryService"/>.
    /// </summary>
    /// <param name="vehicleRepository">
    /// Repositorio utilizado para consultar los vehículos.
    /// </param>
    public VehicleQueryService(
        IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    /// <summary>
    /// Obtiene todos los vehículos disponibles en la flota.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar la operación.
    /// </param>
    /// <returns>
    /// Colección de vehículos en formato DTO.
    /// </returns>
    public async Task<IReadOnlyCollection<VehicleDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var vehicles = await _vehicleRepository.GetAllAsync(
            cancellationToken);

        return vehicles
            .Select(vehicle => new VehicleDto(
                vehicle.Id,
                vehicle.VehicleId,
                vehicle.Name,
                vehicle.Status))
            .ToList();
    }

    /// <summary>
    /// Obtiene un vehículo utilizando su identificador.
    /// </summary>
    /// <param name="vehicleId">
    /// Identificador del vehículo.
    /// </param>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar la operación.
    /// </param>
    /// <returns>
    /// El vehículo encontrado o <c>null</c> si no existe.
    /// </returns>
    public async Task<VehicleDto?> GetByVehicleIdAsync(
        string vehicleId,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleRepository.GetByVehicleIdAsync(
            vehicleId,
            cancellationToken);

        if (vehicle is null)
        {
            return null;
        }

        return new VehicleDto(
            vehicle.Id,
            vehicle.VehicleId,
            vehicle.Name,
            vehicle.Status);
    }
}