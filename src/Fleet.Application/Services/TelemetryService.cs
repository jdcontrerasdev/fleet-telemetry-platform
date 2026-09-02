using Fleet.Application.DTOs;
using Fleet.Application.Interfaces;
using Fleet.Domain.Entities;

namespace Fleet.Application.Services;

/// <summary>
/// Servicio de aplicación encargado de procesar solicitudes de telemetría.
/// </summary>
public sealed class TelemetryService
{
    private readonly IEventPublisher _eventPublisher;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de telemetría.
    /// </summary>
    /// <param name="eventPublisher">
    /// Componente encargado de publicar eventos en el sistema de mensajería.
    /// </param>
    public TelemetryService(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// Acepta una nueva lectura de telemetría y la publica para procesamiento asíncrono.
    /// </summary>
    /// <param name="request">Datos de telemetría recibidos.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Información del evento aceptado.</returns>
    public async Task<TelemetryResponse> IngestAsync(
        TelemetryRequest request,
        CancellationToken cancellationToken = default)
    {
        var telemetry = new Telemetry(
            request.EventId,
            request.VehicleId,
            request.Latitude,
            request.Longitude,
            request.Speed,
            request.Timestamp);

        await _eventPublisher.PublishAsync(
            telemetry,
            cancellationToken);

        return new TelemetryResponse(
            telemetry.EventId,
            "accepted");
    }
}