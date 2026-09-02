using System.Text.Json;
using Confluent.Kafka;
using Fleet.Application.Interfaces;
using Fleet.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fleet.Infrastructure.Messaging;

/// <summary>
/// Consume eventos de telemetría desde Apache Kafka y los entrega
/// al procesador de telemetría para su procesamiento.
/// </summary>
public sealed class KafkaTelemetryConsumer
{
    private readonly KafkaOptions _options;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<KafkaTelemetryConsumer> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="KafkaTelemetryConsumer"/>.
    /// </summary>
    /// <param name="options">
    /// Configuración necesaria para establecer la conexión con Kafka.
    /// </param>
    /// <param name="scopeFactory">
    /// Factoría utilizada para crear scopes de dependencias por mensaje.
    /// </param>
    /// <param name="logger">
    /// Logger utilizado para registrar eventos y errores del consumidor.
    /// </param>
    public KafkaTelemetryConsumer(
        IOptions<KafkaOptions> options,
        IServiceScopeFactory scopeFactory,
        ILogger<KafkaTelemetryConsumer> logger)
    {
        _options = options.Value;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    /// <summary>
    /// Consume continuamente eventos de telemetría desde Kafka.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token utilizado para detener el consumidor.
    /// </param>
    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _options.BootstrapServers,
            GroupId = "fleet-telemetry-worker",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using var consumer =
            new ConsumerBuilder<string, string>(consumerConfig)
                .Build();

        consumer.Subscribe(_options.TelemetryTopic);

        _logger.LogInformation(
            "Kafka consumer iniciado. Topic: {Topic}",
            _options.TelemetryTopic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(cancellationToken);

                    var telemetry = JsonSerializer.Deserialize<Telemetry>(
                        result.Message.Value);

                    if (telemetry is null)
                    {
                        _logger.LogWarning(
                            "Se recibió un mensaje de telemetría vacío.");
                        continue;
                    }

                    using var scope = _scopeFactory.CreateScope();

                    var processor =
                        scope.ServiceProvider
                            .GetRequiredService<ITelemetryProcessor>();

                    await processor.ProcessAsync(
                        telemetry,
                        cancellationToken);

                    consumer.Commit(result);

                    _logger.LogInformation(
                        "Evento de telemetría procesado. EventId: {EventId}",
                        telemetry.EventId);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(
                        ex,
                        "Error consumiendo mensaje desde Kafka.");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error procesando evento de telemetría.");
                }
            }
        }
        finally
        {
            consumer.Close();

            _logger.LogInformation(
                "Kafka consumer detenido.");
        }
    }
}