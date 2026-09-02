using System.Text.Json;
using Confluent.Kafka;
using Fleet.Application.Interfaces;
using Fleet.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fleet.Infrastructure.Messaging;

/// <summary>
/// Consume eventos de telemetría para notificar actualizaciones en tiempo real.
/// </summary>
public sealed class KafkaRealtimeConsumer
{
    private readonly KafkaOptions _options;
    private readonly IRealtimeNotifier _realtimeNotifier;
    private readonly ILogger<KafkaRealtimeConsumer> _logger;

    public KafkaRealtimeConsumer(
        IOptions<KafkaOptions> options,
        IRealtimeNotifier realtimeNotifier,
        ILogger<KafkaRealtimeConsumer> logger)
    {
        _options = options.Value;
        _realtimeNotifier = realtimeNotifier;
        _logger = logger;
    }

    /// <summary>
    /// Consume eventos de telemetría y los publica para las conexiones SSE.
    /// </summary>
    public async Task ConsumeAsync(
        CancellationToken cancellationToken)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _options.BootstrapServers,
            GroupId = "fleet-realtime-api",
            AutoOffsetReset = AutoOffsetReset.Latest,
            EnableAutoCommit = false
        };

        using var consumer =
            new ConsumerBuilder<string, string>(consumerConfig).Build();

        consumer.Subscribe(_options.TelemetryTopic);

        _logger.LogInformation(
            "Kafka realtime consumer iniciado. Topic: {Topic}",
            _options.TelemetryTopic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(cancellationToken);

                    var telemetry =
                        JsonSerializer.Deserialize<Telemetry>(
                            result.Message.Value);

                    if (telemetry is null)
                    {
                        _logger.LogWarning(
                            "Se recibió un evento de telemetría vacío.");
                        continue;
                    }

                    await _realtimeNotifier.NotifyAsync(
                        telemetry.VehicleId,
                        cancellationToken);

                    consumer.Commit(result);

                    _logger.LogInformation(
                        "Evento enviado a SSE. VehicleId: {VehicleId}",
                        telemetry.VehicleId);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(
                        ex,
                        "Error consumiendo evento para realtime.");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error procesando evento para realtime.");
                }
            }
        }
        finally
        {
            consumer.Close();

            _logger.LogInformation(
                "Kafka realtime consumer detenido.");
        }
    }
}