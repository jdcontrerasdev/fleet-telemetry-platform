using Fleet.Infrastructure;
using Fleet.Application.Services;
using Fleet.Application.DTOs;
using Fleet.Infrastructure.Persistence;
using Fleet.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

Fleet.Infrastructure.DependencyInjection.AddInfrastructure(
    builder.Services,
    builder.Configuration);
    
builder.Services.AddScoped<TelemetryService>();
builder.Services.AddScoped<TelemetryQueryService>();
builder.Services.AddScoped<VehicleStateQueryService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<FleetDbContext>();

    await VehicleSeeder.SeedAsync(dbContext);
}

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

app.MapGet(
    "/api/vehicles",
    async (
        VehicleQueryService vehicleQueryService,
        CancellationToken cancellationToken) =>
    {
        var vehicles = await vehicleQueryService.GetAllAsync(
            cancellationToken);

        return Results.Ok(vehicles);
    });

app.MapGet(
    "/api/vehicles/{vehicleId}",
    async (
        string vehicleId,
        VehicleQueryService vehicleQueryService,
        CancellationToken cancellationToken) =>
    {
        var vehicle = await vehicleQueryService.GetByVehicleIdAsync(
            vehicleId,
            cancellationToken);

        return vehicle is null
            ? Results.NotFound()
            : Results.Ok(vehicle);
    });

app.MapGet(
    "/api/vehicles/{vehicleId}/telemetry",
    async (
        string vehicleId,
        TelemetryQueryService telemetryQueryService,
        CancellationToken cancellationToken) =>
    {
        var telemetry = await telemetryQueryService.GetByVehicleIdAsync(
            vehicleId,
            cancellationToken);

        return Results.Ok(telemetry);
    });

app.MapGet(
    "/api/vehicles/{vehicleId}/state",
    async (
        string vehicleId,
        VehicleStateQueryService vehicleStateQueryService,
        CancellationToken cancellationToken) =>
    {
        var state = await vehicleStateQueryService.GetByVehicleIdAsync(
            vehicleId,
            cancellationToken);

        return state is null
            ? Results.NotFound()
            : Results.Ok(state);
    });

app.Run();