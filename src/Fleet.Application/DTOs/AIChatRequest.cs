namespace Fleet.Application.DTOs;

/// <summary>
/// Representa una consulta enviada al agente de IA.
/// </summary>
public sealed record AIChatRequest(
    string Message);