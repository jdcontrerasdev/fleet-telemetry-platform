using System.Threading.Channels;
using Fleet.Application.Interfaces;

namespace Fleet.Infrastructure.Realtime;

/// <summary>
/// Implementa la notificación de eventos para conexiones SSE.
/// </summary>
public sealed class SseRealtimeNotifier : IRealtimeNotifier
{
    private readonly Channel<string> _channel =
        Channel.CreateUnbounded<string>();

    /// <summary>
    /// Publica una actualización de telemetría.
    /// </summary>
    public async Task NotifyAsync(
        string vehicleId,
        CancellationToken cancellationToken = default)
    {
        await _channel.Writer.WriteAsync(
            vehicleId,
            cancellationToken);
    }

    /// <summary>
    /// Obtiene los eventos publicados en tiempo real.
    /// </summary>
    public IAsyncEnumerable<string> ReadAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }
}