using Fleet.Infrastructure;
using Fleet.TelemetryWorker;

var builder = Host.CreateApplicationBuilder(args);

Fleet.Infrastructure.DependencyInjection.AddInfrastructure(
    builder.Services,
    builder.Configuration);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

host.Run();
