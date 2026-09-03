using Fleet.Domain.Entities;

namespace Fleet.Application.Interfaces;

/// <summary>
/// Define las operaciones de acceso al estado actual de los vehículos.
/// </summary>
public interface IVehicleStateRepository
{
    /// <summary>
    /// Obtiene el estado actual de un vehículo.
    /// </summary>
    Task<VehicleState?> GetByVehicleIdAsync(
        string vehicleId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene el estado actual de todos los vehículos.
    /// </summary>
    Task<IReadOnlyCollection<VehicleState>> GetAllAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Crea o actualiza el estado de un vehículo.
    /// </summary>
    Task UpsertAsync(
        VehicleState vehicleState,
        CancellationToken cancellationToken = default);
}