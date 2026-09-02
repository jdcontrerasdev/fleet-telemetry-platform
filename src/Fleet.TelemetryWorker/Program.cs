using Fleet.TelemetryWorker;
using Fleet.Application.Interfaces;
using Fleet.Infrastructure.Realtime;

var builder = Host.CreateApplicationBuilder(args);

Fleet.Infrastructure.DependencyInjection.AddInfrastructure(
    builder.Services,
    builder.Configuration);

builder.Services.AddSingleton<SseRealtimeNotifier>();

builder.Services.AddSingleton<IRealtimeNotifier>(
    sp => sp.GetRequiredService<SseRealtimeNotifier>());
    
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

host.Run();
