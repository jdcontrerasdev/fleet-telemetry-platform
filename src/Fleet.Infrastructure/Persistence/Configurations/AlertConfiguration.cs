using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de persistencia para la entidad Alert.
/// </summary>
public sealed class AlertConfiguration : IEntityTypeConfiguration<Alert>
{
    /// <summary>
    /// Configura el mapeo de la entidad Alert.
    /// </summary>
    /// <param name="builder">Constructor de configuración de la entidad.</param>
    public void Configure(EntityTypeBuilder<Alert> builder)
    {
        builder.ToTable("alerts");

        builder.HasKey(alert => alert.Id);

        builder.Property(alert => alert.VehicleId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(alert => alert.Message)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(alert => alert.Severity)
            .IsRequired();

        builder.Property(alert => alert.CreatedAt)
            .IsRequired();

        builder.Property(alert => alert.IsResolved)
            .IsRequired();

        builder.HasIndex(alert => new
        {
            alert.VehicleId,
            alert.IsResolved,
            alert.CreatedAt
        });
    }
}