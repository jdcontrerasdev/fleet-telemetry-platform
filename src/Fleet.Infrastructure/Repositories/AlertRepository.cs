using Fleet.Application.Interfaces;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Infrastructure.Repositories;

/// <summary>
/// Implementa la persistencia de alertas.
/// </summary>
public sealed class AlertRepository : IAlertRepository
{
    private readonly FleetDbContext _dbContext;

    /// <summary>
    /// Inicializa el repositorio de alertas.
    /// </summary>
    public AlertRepository(FleetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene las alertas ordenadas de los vehiculos desde la más reciente.
    /// </summary>
    public async Task<IReadOnlyCollection<Alert>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Alerts
            .AsNoTracking()
            .OrderByDescending(alert => alert.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Agrega una alerta a la unidad de trabajo actual.
    /// </summary>
    public async Task AddAsync(
        Alert alert,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Alerts.AddAsync(
            alert,
            cancellationToken);
    }
}