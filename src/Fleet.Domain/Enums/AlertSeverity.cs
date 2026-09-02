namespace Fleet.Domain.Enums;

/// <summary>
/// Representa el nivel de severidad de una alerta operativa.
/// </summary>
public enum AlertSeverity
{
    /// <summary>
    /// Alerta de baja prioridad.
    /// </summary>
    Low = 1,

    /// <summary>
    /// Alerta de prioridad media.
    /// </summary>
    Medium = 2,

    /// <summary>
    /// Alerta de alta prioridad.
    /// </summary>
    High = 3,

    /// <summary>
    /// Alerta de máxima prioridad que requiere atención inmediata.
    /// </summary>
    Critical = 4
}