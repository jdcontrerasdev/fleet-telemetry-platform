using System.Text.Json;
using Confluent.Kafka;
using Fleet.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace Fleet.Infrastructure.Messaging;

/// <summary>
/// Implementa la publicación de eventos de aplicación utilizando Apache Kafka.
/// </summary>
public sealed class KafkaEventPublisher : IEventPublisher
{
    private readonly KafkaOptions _options;
    private readonly IProducer<string, string> _producer;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="KafkaEventPublisher"/>.
    /// </summary>
    /// <param name="options">
    /// Configuración necesaria para establecer la conexión con Kafka.
    /// </param>
    public KafkaEventPublisher(IOptions<KafkaOptions> options)
    {
        _options = options.Value;

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _options.BootstrapServers
        };

        _producer = new ProducerBuilder<string, string>(producerConfig)
            .Build();
    }

    /// <summary>
    /// Serializa y publica un evento en el topic configurado de Kafka.
    /// </summary>
    /// <typeparam name="T">Tipo del evento que será publicado.</typeparam>
    /// <param name="eventMessage">Evento que será serializado y publicado.</param>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar la operación de publicación.
    /// </param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    public async Task PublishAsync<T>(
        T eventMessage,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(eventMessage);

        await _producer.ProduceAsync(
            _options.TelemetryTopic,
            new Message<string, string>
            {
                Value = json
            },
            cancellationToken);
    }
}