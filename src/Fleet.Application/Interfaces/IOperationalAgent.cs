namespace Fleet.Application.Interfaces;

/// <summary>
/// Define las operaciones del agente de IA para consultas operativas.
/// </summary>
public interface IOperationalAgent
{
    /// <summary>
    /// Procesa una consulta operativa.
    /// </summary>
    Task<string> AskAsync(
        string message,
        CancellationToken cancellationToken = default);
}