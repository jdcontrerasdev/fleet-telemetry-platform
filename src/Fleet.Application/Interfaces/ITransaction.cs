namespace Fleet.Application.Interfaces;

/// <summary>
/// Representa una transacción de persistencia.
/// </summary>
public interface ITransaction : IAsyncDisposable
{
    Task CommitAsync(
        CancellationToken cancellationToken = default);

    Task RollbackAsync(
        CancellationToken cancellationToken = default);
}