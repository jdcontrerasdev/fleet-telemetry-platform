using Fleet.Infrastructure.Messaging;

namespace Fleet.TelemetryWorker;

/// <summary>
/// Servicio en segundo plano encargado de ejecutar el consumidor
/// de eventos de telemetría.
/// </summary>
public sealed class Worker : BackgroundService
{
    private readonly KafkaTelemetryConsumer _consumer;
    private readonly ILogger<Worker> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="Worker"/>.
    /// </summary>
    /// <param name="consumer">
    /// Consumidor encargado de recibir eventos desde Kafka.
    /// </param>
    /// <param name="logger">
    /// Logger utilizado para registrar información del worker.
    /// </param>
    public Worker(
        KafkaTelemetryConsumer consumer,
        ILogger<Worker> logger)
    {
        _consumer = consumer;
        _logger = logger;
    }

    /// <summary>
    /// Ejecuta el procesamiento de eventos mientras la aplicación
    /// permanezca activa.
    /// </summary>
    /// <param name="stoppingToken">
    /// Token utilizado para detener el servicio.
    /// </param>
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Fleet Telemetry Worker iniciado.");

        await _consumer.ConsumeAsync(stoppingToken);

        _logger.LogInformation(
            "Fleet Telemetry Worker detenido.");
    }
}