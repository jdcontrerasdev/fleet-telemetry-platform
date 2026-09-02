using Fleet.Domain.Enums;

namespace Fleet.Domain.Entities;

/// <summary>
/// Representa una alerta operativa generada para un vehículo de la flota.
/// </summary>
public class Alert
{
    /// <summary>
    /// Obtiene el identificador único de la alerta.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Obtiene el identificador de negocio del vehículo afectado.
    /// </summary>
    public string VehicleId { get; private set; } = string.Empty;

    /// <summary>
    /// Obtiene la descripción de la alerta.
    /// </summary>
    public string Message { get; private set; } = string.Empty;

    /// <summary>
    /// Obtiene el nivel de severidad de la alerta.
    /// </summary>
    public AlertSeverity Severity { get; private set; }

    /// <summary>
    /// Obtiene la fecha y hora en que se creó la alerta.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Indica si la alerta ya fue resuelta.
    /// </summary>
    public bool IsResolved { get; private set; }

    /// <summary>
    /// Constructor requerido por Entity Framework Core.
    /// </summary>
    private Alert()
    {
    }

    /// <summary>
    /// Crea una nueva alerta operativa.
    /// </summary>
    /// <param name="vehicleId">Identificador de negocio del vehículo afectado.</param>
    /// <param name="message">Descripción de la alerta.</param>
    /// <param name="severity">Nivel de severidad de la alerta.</param>
    public Alert(
        string vehicleId,
        string message,
        AlertSeverity severity)
    {
        if (string.IsNullOrWhiteSpace(vehicleId))
            throw new ArgumentException(
                "El identificador del vehículo es obligatorio.",
                nameof(vehicleId));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException(
                "El mensaje de la alerta es obligatorio.",
                nameof(message));

        Id = Guid.NewGuid();
        VehicleId = vehicleId;
        Message = message;
        Severity = severity;
        CreatedAt = DateTime.UtcNow;
        IsResolved = false;
    }

    /// <summary>
    /// Marca la alerta como resuelta.
    /// </summary>
    public void Resolve()
    {
        IsResolved = true;
    }
}