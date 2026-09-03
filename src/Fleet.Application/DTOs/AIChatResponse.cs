namespace Fleet.Application.DTOs;

/// <summary>
/// Representa la respuesta del agente de IA.
/// </summary>
public sealed record AIChatResponse(
    string Answer);