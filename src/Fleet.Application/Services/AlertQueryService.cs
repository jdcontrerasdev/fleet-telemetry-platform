using Fleet.Application.DTOs;
using Fleet.Application.Interfaces;

namespace Fleet.Application.Services;

/// <summary>
/// Proporciona operaciones de consulta relacionadas
/// con las alertas de los vehiculos.
/// </summary>
public sealed class AlertQueryService
{
    private readonly IAlertRepository _alertRepository;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="AlertQueryService"/>.
    /// </summary>
    /// <param name="alertRepository">
    /// Repositorio utilizado para consultar las alertas.
    /// </param>
    public AlertQueryService(
        IAlertRepository alertRepository)
    {
        _alertRepository = alertRepository;
    }

    /// <summary>
    /// Obtiene las alertas registradas en los vehiculos.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar la operación asíncrona.
    /// </param>
    /// <returns>
    /// Colección de alertas ordenadas desde la más reciente.
    /// </returns>
    public async Task<IReadOnlyCollection<AlertDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var alerts = await _alertRepository.GetAllAsync(
            cancellationToken);

        return alerts
            .Select(alert => new AlertDto(
                alert.Id,
                alert.VehicleId,
                alert.Message,
                alert.Severity,
                alert.CreatedAt,
                alert.IsResolved))
            .ToList();
    }
}