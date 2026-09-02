using Fleet.Infrastructure;
using Fleet.Application.Services;
using Fleet.Application.DTOs;

var builder = WebApplication.CreateBuilder(args);

Fleet.Infrastructure.DependencyInjection.AddInfrastructure(
    builder.Services,
    builder.Configuration);
    
builder.Services.AddScoped<TelemetryService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost(
    "/api/telemetry",
    async (
        TelemetryRequest request,
        TelemetryService telemetryService,
        CancellationToken cancellationToken) =>
    {
        var response = await telemetryService.IngestAsync(
            request,
            cancellationToken);

        return Results.Accepted(
            $"/api/telemetry/{response.EventId}",
            response);
    });

app.Run();