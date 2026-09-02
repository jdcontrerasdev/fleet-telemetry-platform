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
    /// <param name="vehicleId">Identificador de negocio del vehículo.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Estado actual o <c>null</c> si no existe.</returns>
    Task<VehicleState?> GetByVehicleIdAsync(
        string vehicleId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Guarda o actualiza el estado actual de un vehículo.
    /// </summary>
    /// <param name="vehicleState">Estado actual del vehículo.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    Task UpsertAsync(
        VehicleState vehicleState,
        CancellationToken cancellationToken = default);
}