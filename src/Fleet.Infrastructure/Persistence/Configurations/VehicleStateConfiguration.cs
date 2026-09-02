using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de persistencia para la entidad VehicleState.
/// </summary>
public sealed class VehicleStateConfiguration : IEntityTypeConfiguration<VehicleState>
{
    /// <summary>
    /// Configura el mapeo de la entidad VehicleState.
    /// </summary>
    /// <param name="builder">Constructor de configuración de la entidad.</param>
    public void Configure(EntityTypeBuilder<VehicleState> builder)
    {
        builder.ToTable("vehicle_state");

        builder.HasKey(state => state.VehicleId);

        builder.Property(state => state.VehicleId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(state => state.Latitude)
            .IsRequired();

        builder.Property(state => state.Longitude)
            .IsRequired();

        builder.Property(state => state.Speed)
            .IsRequired();

        builder.Property(state => state.LastTelemetryAt)
            .IsRequired();
    }
}