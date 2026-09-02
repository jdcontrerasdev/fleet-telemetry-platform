using Fleet.Infrastructure.Messaging;

namespace Fleet.Api.Realtime;

/// <summary>
/// Ejecuta el consumidor Kafka encargado de las notificaciones en tiempo real.
/// </summary>
public sealed class KafkaRealtimeWorker : BackgroundService
{
    private readonly KafkaRealtimeConsumer _consumer;

    public KafkaRealtimeWorker(KafkaRealtimeConsumer consumer)
    {
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(
    CancellationToken stoppingToken)
    {
        await Task.Yield();

        await _consumer.ConsumeAsync(stoppingToken);
    }
}