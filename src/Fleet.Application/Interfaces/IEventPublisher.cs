namespace Fleet.Application.Interfaces;

/// <summary>
/// Define el contrato para publicar eventos de integración.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publica un evento en el sistema de mensajería.
    /// </summary>
    /// <typeparam name="T">Tipo del evento a publicar.</typeparam>
    /// <param name="eventMessage">Evento que será publicado.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    Task PublishAsync<T>(
        T eventMessage,
        CancellationToken cancellationToken = default);
}