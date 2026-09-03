namespace Fleet.Infrastructure.AI;

/// <summary>
/// Configuración del proveedor de IA.
/// </summary>
public sealed class AIOptions
{
    public string ApiKey { get; set; } = string.Empty;

    public string Model { get; set; } = "gemini-3.7-flash";
}