using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Infrastructure.Persistence;

/// <summary>
/// Contexto de persistencia de la plataforma de telemetría.
/// </summary>
public class FleetDbContext : DbContext
{
    /// <summary>
    /// Inicializa una nueva instancia del contexto de base de datos.
    /// </summary>
    /// <param name="options">Opciones de configuración del contexto.</param>
    public FleetDbContext(DbContextOptions<FleetDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Obtiene el conjunto de vehículos.
    /// </summary>
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    /// <summary>
    /// Obtiene el conjunto de eventos de telemetría.
    /// </summary>
    public DbSet<Telemetry> Telemetries => Set<Telemetry>();

    /// <summary>
    /// Obtiene el conjunto de estados actuales de los vehículos.
    /// </summary>
    public DbSet<VehicleState> VehicleStates => Set<VehicleState>();

    /// <summary>
    /// Obtiene el conjunto de alertas.
    /// </summary>
    public DbSet<Alert> Alerts => Set<Alert>();

    /// <summary>
    /// Configura el modelo de persistencia de la aplicación.
    /// </summary>
    /// <param name="modelBuilder">Constructor del modelo de EF Core.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /// ApplyConfigurationsFromAssembly para mantener cada configuración separada, hace que FleetDbContext permanezca limpio y facilita evolucionar el modelo
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(FleetDbContext).Assembly);
    }
}