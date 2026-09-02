using Fleet.Domain.Entities;

namespace Fleet.Application.Interfaces;

/// <summary>
/// Define las operaciones de persistencia de alertas.
/// </summary>
public interface IAlertRepository
{
    /// <summary>
    /// Obtiene todas las alertas registradas.
    /// </summary>
    Task<IReadOnlyCollection<Alert>> GetAllAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Agrega una nueva alerta.
    /// </summary>
    Task AddAsync(
        Alert alert,
        CancellationToken cancellationToken = default);
}