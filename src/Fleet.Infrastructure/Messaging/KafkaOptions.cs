namespace Fleet.Infrastructure.Messaging;

/// <summary>
/// Representa la configuración necesaria para la conexión y publicación de eventos en Kafka.
/// </summary>
public sealed class KafkaOptions
{
    /// <summary>
    /// Obtiene o establece los servidores Kafka utilizados como punto de conexión.
    /// </summary>
    public string BootstrapServers { get; set; } = "localhost:9092";

    /// <summary>
    /// Obtiene o establece el nombre del topic utilizado para los eventos de telemetría.
    /// </summary>
    public string TelemetryTopic { get; set; } = "fleet.telemetry";
}