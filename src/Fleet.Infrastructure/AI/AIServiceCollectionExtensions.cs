using Fleet.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fleet.Infrastructure.AI;

/// <summary>
/// Configura los servicios necesarios para el agente de IA.
/// </summary>
public static class AIServiceCollectionExtensions
{
    /// <summary>
    /// Registra los servicios del agente de IA.
    /// </summary>
    public static IServiceCollection AddFleetAI(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AIOptions>(
            configuration.GetSection("AI"));

        services.AddScoped<OperationalAgent>();

        services.AddScoped<IOperationalAgent>(
            serviceProvider =>
                serviceProvider.GetRequiredService<OperationalAgent>());

        return services;
    }
}