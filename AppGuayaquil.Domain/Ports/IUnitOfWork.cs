namespace AppGuayaquil.Domain.Ports;

public interface IUnitOfWork
{
    Task SaveAsync();
    Task SaveAsync(CancellationToken cancellationToken);
}
