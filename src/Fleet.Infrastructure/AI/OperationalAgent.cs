using System.Text.Json;
using Fleet.Application.Interfaces;
using Fleet.Application.Services;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Options;

namespace Fleet.Infrastructure.AI;

/// <summary>
/// Implementa el agente de IA para consultas operativas de la flota.
/// </summary>
public sealed class OperationalAgent : IOperationalAgent
{
    private readonly VehicleOperationalQueryService _queryService;
    private readonly AIOptions _options;
    private readonly Client _client;

    public OperationalAgent(
        VehicleOperationalQueryService queryService,
        IOptions<AIOptions> options)
    {
        _queryService = queryService;
        _options = options.Value;

        _client = new Client(
            apiKey: _options.ApiKey);
    }

/// <summary>
/// Procesa una consulta operativa utilizando Gemini para retornar una respuesta mas natural
/// </summary>
public async Task<string> AskAsync(
        string message,
        CancellationToken cancellationToken = default)
    {
        var tool = new Tool
        {
            FunctionDeclarations =
            [
                new FunctionDeclaration
                {
                    Name = "get_stopped_vehicles",
                    Description =
                        "Obtiene los vehículos que llevan detenidos " +
                        "más tiempo del indicado.",
                    Parameters = new Schema
                    {
                        Type = Google.GenAI.Types.Type.Object,
                        Properties = new Dictionary<string, Schema>
                        {
                            ["minutes"] = new Schema
                            {
                                Type = Google.GenAI.Types.Type.Integer,
                                Description =
                                    "Cantidad de minutos desde los cuales " +
                                    "se considera que un vehículo está detenido."
                            }
                        },
                        Required = ["minutes"]
                    }
                }
            ]
        };

        var config = new GenerateContentConfig
        {
            Tools = [tool]
        };

        var response = await _client.Models.GenerateContentAsync(
            _options.Model,
            message,
            config,
            cancellationToken);

        if (response.FunctionCalls is null ||
            response.FunctionCalls.Count == 0)
        {
            return response.Text ?? "No se pudo generar una respuesta.";
        }

        var functionCall = response.FunctionCalls[0];

        if (functionCall.Name != "get_stopped_vehicles")
        {
            return "La operación solicitada no está disponible.";
        }

        var minutes = GetMinutes(functionCall.Args);

        var result = await GetStoppedVehiclesAsync(
            minutes,
            cancellationToken);

        var functionResponse = new Content
        {
            Role = "user",
            Parts =
            [
                new Part
                {
                    FunctionResponse = new FunctionResponse
                    {
                        Name = functionCall.Name,
                        Response = new Dictionary<string, object>
                        {
                            ["result"] = result
                        }
                    }
                }
            ]
        };

        var finalResponse = await _client.Models.GenerateContentAsync(
            _options.Model,
            [
                new Content
                {
                    Role = "user",
                    Parts =
                    [
                        new Part
                        {
                            Text = message
                        }
                    ]
                },
                response.Candidates![0].Content!,
                functionResponse
            ],
            config,
            cancellationToken);

        return finalResponse.Text ??
            "No se pudo generar una respuesta.";
    }

    /// <summary>
    /// Ejecuta la consulta de vehículos detenidos.
    /// </summary>
    private async Task<string> GetStoppedVehiclesAsync(
        int minutes,
        CancellationToken cancellationToken)
    {
        var vehicles = await _queryService.GetStoppedVehiclesAsync(
            minutes,
            cancellationToken);

        if (vehicles.Count == 0)
        {
            return "No hay vehículos detenidos durante ese periodo.";
        }

        return string.Join(
            System.Environment.NewLine,
            vehicles.Select(vehicle =>
                $"- {vehicle.VehicleId}: detenido desde " +
                $"{vehicle.LastTelemetryAt:yyyy-MM-dd HH:mm:ss} UTC"));
    }

    /// <summary>
    /// Obtiene el parámetro minutes enviado por Gemini.
    /// </summary>
    private static int GetMinutes(
        Dictionary<string, object>? arguments)
    {
        if (arguments is null ||
            !arguments.TryGetValue("minutes", out var value))
        {
            return 20;
        }

        return value switch
        {
            int intValue => intValue,
            long longValue => (int)longValue,
            double doubleValue => (int)doubleValue,
            JsonElement jsonElement when
                jsonElement.TryGetInt32(out var intValue)
                => intValue,
            _ => 20
        };
    }
}