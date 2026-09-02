using Fleet.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configura la persistencia de los eventos de telemetría procesados.
/// </summary>
public sealed class ProcessedTelemetryEventConfiguration
    : IEntityTypeConfiguration<ProcessedTelemetryEvent>
{
    public void Configure(
        EntityTypeBuilder<ProcessedTelemetryEvent> builder)
    {
        builder.ToTable("processed_telemetry_events");

        builder.HasKey(processedEvent =>
            processedEvent.EventId);

        builder.Property(processedEvent =>
            processedEvent.EventId)
            .IsRequired();

        builder.Property(processedEvent =>
            processedEvent.ProcessedAt)
            .IsRequired();
    }
}