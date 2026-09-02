namespace Fleet.Application.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task<ITransaction> BeginTransactionAsync(
        CancellationToken cancellationToken = default);
}