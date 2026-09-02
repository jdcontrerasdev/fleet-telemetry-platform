using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de persistencia para la entidad Vehicle.
/// </summary>
public sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    /// <summary>
    /// Configura el mapeo de la entidad Vehicle.
    /// </summary>
    /// <param name="builder">Constructor de configuración de la entidad.</param>
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("vehicles");

        builder.HasKey(vehicle => vehicle.Id);

        builder.Property(vehicle => vehicle.VehicleId)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(vehicle => vehicle.VehicleId)
            .IsUnique();

        builder.Property(vehicle => vehicle.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(vehicle => vehicle.Status)
            .IsRequired();

        builder.Property(vehicle => vehicle.CreatedAt)
            .IsRequired();
    }
}