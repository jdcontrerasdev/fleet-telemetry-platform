using Fleet.Application.Interfaces;
using Fleet.Domain.Entities;

namespace Fleet.Application.Services;

/// <summary>
/// Procesa eventos de telemetría y coordina su persistencia
/// junto con la actualización del estado actual del vehículo.
/// </summary>
public sealed class TelemetryProcessor : ITelemetryProcessor
{
    private readonly ITelemetryRepository _telemetryRepository;
    private readonly IVehicleStateRepository _vehicleStateRepository;
    private readonly IProcessedTelemetryEventRepository _processedEventRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="TelemetryProcessor"/>.
    /// </summary>
    /// <param name="telemetryRepository">
    /// Repositorio utilizado para almacenar la telemetría histórica.
    /// </param>
    /// <param name="vehicleStateRepository">
    /// Repositorio utilizado para actualizar el estado actual del vehículo.
    /// </param>
    /// <param name="processedEventRepository">
    /// Repositorio utilizado para controlar la idempotencia
    /// de los eventos procesados.
    /// </param>
    /// <param name="unitOfWork">
    /// Unidad de trabajo utilizada para gestionar la transacción
    /// y confirmar los cambios de persistencia.
    /// </param>   
    public TelemetryProcessor(
        ITelemetryRepository telemetryRepository,
        IVehicleStateRepository vehicleStateRepository,
        IProcessedTelemetryEventRepository processedEventRepository,
        IUnitOfWork unitOfWork)
    {
        _telemetryRepository = telemetryRepository;
        _vehicleStateRepository = vehicleStateRepository;
        _processedEventRepository = processedEventRepository;
        _unitOfWork = unitOfWork;        
    }

    /// <summary>
    /// Procesa un evento de telemetría de forma transaccional e idempotente.
    /// </summary>
    /// <param name="telemetry">
    /// Evento de telemetría recibido desde Kafka.
    /// </param>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar la operación.
    /// </param>
    public async Task ProcessAsync(
        Telemetry telemetry,
        CancellationToken cancellationToken = default)
    {
        await using var transaction =
            await _unitOfWork.BeginTransactionAsync(
                cancellationToken);

        var registered = await _processedEventRepository.TryRegisterAsync(
            telemetry.EventId,
            cancellationToken);

        if (!registered)
        {
            await transaction.RollbackAsync(cancellationToken);
            return;
        }

        await _telemetryRepository.AddAsync(
            telemetry,
            cancellationToken);

        var vehicleState = new VehicleState(
            telemetry.VehicleId,
            telemetry.Latitude,
            telemetry.Longitude,
            telemetry.Speed,
            telemetry.Timestamp);

        await _vehicleStateRepository.UpsertAsync(
            vehicleState,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        await transaction.CommitAsync(
            cancellationToken);        
    }
}