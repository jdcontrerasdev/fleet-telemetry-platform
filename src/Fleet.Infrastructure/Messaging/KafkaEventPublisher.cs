using System.Text.Json;
using Confluent.Kafka;
using Fleet.Application.Interfaces;
using Microsoft.Extensions.Options;
using Fleet.Domain.Entities;

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
                Key = GetMessageKey(eventMessage),
                Value = json
            },
            cancellationToken);
            }

    /// <summary>
    /// Obtiene la clave utilizada para determinar la partición
    /// del evento dentro de Kafka.
    /// </summary>
    /// <typeparam name="T">
    /// Tipo del evento publicado.
    /// </typeparam>
    /// <param name="eventMessage">
    /// Evento del cual se obtiene la clave.
    /// </param>
    /// <returns>
    /// Identificador del vehículo cuando el evento corresponde a telemetría.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Se produce cuando el evento no corresponde a un tipo soportado.
    /// </exception>
    private static string GetMessageKey<T>(T eventMessage)
    {
        if (eventMessage is Telemetry telemetry)
        {
            return telemetry.VehicleId;
        }

        throw new ArgumentException(
            $"El tipo de evento '{typeof(T).Name}' no es soportado.",
            nameof(eventMessage));
    }
}