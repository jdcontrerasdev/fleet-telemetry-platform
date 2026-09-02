namespace Fleet.Application.Interfaces;

/// <summary>
/// Define la notificación de eventos en tiempo real.
/// </summary>
public interface IRealtimeNotifier
{
    /// <summary>
    /// Notifica una actualización de telemetría.
    /// </summary>
    Task NotifyAsync(
        string vehicleId,
        CancellationToken cancellationToken = default);
}