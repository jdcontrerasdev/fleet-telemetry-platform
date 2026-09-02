using Fleet.Domain.Enums;

namespace Fleet.Domain.Entities;

/// <summary>
/// Representa un vehículo registrado dentro de la flota.
/// </summary>
public class Vehicle
{
    /// <summary>
    /// Obtiene el identificador único interno del vehículo.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Obtiene el identificador de negocio asignado al vehículo.
    /// Ejemplo: TRUCK-001.
    /// </summary>
    public string VehicleId { get; private set; } = string.Empty;

    /// <summary>
    /// Obtiene el nombre descriptivo del vehículo.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Obtiene el estado operativo actual del vehículo.
    /// </summary>
    public VehicleStatus Status { get; private set; }

    /// <summary>
    /// Obtiene la fecha y hora en que el vehículo fue registrado.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Constructor requerido por Entity Framework Core.
    /// </summary>
    private Vehicle()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de un vehículo.
    /// </summary>
    /// <param name="vehicleId">Identificador de negocio del vehículo.</param>
    /// <param name="name">Nombre descriptivo del vehículo.</param>
    public Vehicle(string vehicleId, string name)
    {
        if (string.IsNullOrWhiteSpace(vehicleId))
            throw new ArgumentException(
                "El identificador del vehículo es obligatorio.",
                nameof(vehicleId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "El nombre del vehículo es obligatorio.",
                nameof(name));

        Id = Guid.NewGuid();
        VehicleId = vehicleId;
        Name = name;
        Status = VehicleStatus.Offline;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Actualiza el estado operativo actual del vehículo.
    /// </summary>
    /// <param name="status">Nuevo estado del vehículo.</param>
    public void UpdateStatus(VehicleStatus status)
    {
        Status = status;
    }
}