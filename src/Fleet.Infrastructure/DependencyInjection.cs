using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fleet.Infrastructure;

/// <summary>
/// Métodos de extensión para registrar los servicios de infraestructura.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra los servicios de persistencia de la plataforma.
    /// </summary>
    /// <param name="services">Colección de servicios de la aplicación.</param>
    /// <param name="configuration">Configuración de la aplicación.</param>
    /// <returns>La colección de servicios para continuar configurando DI.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("FleetDatabase");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "La cadena de conexión 'FleetDatabase' no está configurada.");
        }

        services.AddDbContext<FleetDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}