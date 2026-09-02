using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de persistencia para la entidad Telemetry.
/// </summary>
public sealed class TelemetryConfiguration : IEntityTypeConfiguration<Telemetry>
{
    /// <summary>
    /// Configura el mapeo de la entidad Telemetry.
    /// </summary>
    /// <param name="builder">Constructor de configuración de la entidad.</param>
    public void Configure(EntityTypeBuilder<Telemetry> builder)
    {
        builder.ToTable("telemetry");

        builder.HasKey(telemetry => telemetry.EventId);

        builder.Property(telemetry => telemetry.VehicleId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(telemetry => telemetry.Latitude)
            .IsRequired();

        builder.Property(telemetry => telemetry.Longitude)
            .IsRequired();

        builder.Property(telemetry => telemetry.Speed)
            .IsRequired();

        builder.Property(telemetry => telemetry.Timestamp)
            .IsRequired();

        builder.HasIndex(telemetry => new
        {
            telemetry.VehicleId,
            telemetry.Timestamp
        });
    }
}