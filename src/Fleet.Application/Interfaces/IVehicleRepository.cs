using Fleet.Domain.Entities;

namespace Fleet.Application.Interfaces;

/// <summary>
/// Define las operaciones de acceso a vehículos.
/// </summary>
public interface IVehicleRepository
{
    /// <summary>
    /// Obtiene todos los vehículos registrados.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Lista de vehículos.</returns>
    Task<IReadOnlyCollection<Vehicle>> GetAllAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un vehículo por su identificador de negocio.
    /// </summary>
    /// <param name="vehicleId">Identificador de negocio.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>El vehículo encontrado o <c>null</c>.</returns>
    Task<Vehicle?> GetByVehicleIdAsync(
        string vehicleId,
        CancellationToken cancellationToken = default);
}