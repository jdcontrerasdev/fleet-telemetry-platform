using Fleet.Application.Interfaces;
using Fleet.Infrastructure.Messaging;
using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fleet.Infrastructure.Repositories;
using Fleet.Application.Services;

namespace Fleet.Infrastructure;

/// <summary>
/// Proporciona métodos de extensión para registrar los servicios
/// relacionados con la infraestructura de la aplicación.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra los servicios de infraestructura necesarios para la aplicación.
    /// </summary>
    /// <param name="services">
    /// Colección de servicios utilizada por el contenedor de inyección de dependencias.
    /// </param>
    /// <param name="configuration">
    /// Configuración de la aplicación.
    /// </param>
    /// <returns>
    /// La colección de servicios con las dependencias de infraestructura registradas.
    /// </returns>
    public static IServiceCollection AddInfrastructure(
        IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("FleetDatabase");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "La cadena de conexión 'FleetDatabase' no está configurada.");
        }

        services.AddDbContext<FleetDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.Configure<KafkaOptions>(
            configuration.GetSection("Kafka"));

        services.AddSingleton<KafkaTelemetryConsumer>();
        services.AddSingleton<IEventPublisher, KafkaEventPublisher>();
        services.AddScoped<ITelemetryRepository, TelemetryRepository>();
        services.AddScoped<IVehicleStateRepository, VehicleStateRepository>();
        services.AddScoped<ITelemetryProcessor, TelemetryProcessor>();
        
        services.AddScoped<IUnitOfWork>(
            serviceProvider =>
                serviceProvider.GetRequiredService<FleetDbContext>());

        return services;
    }
}