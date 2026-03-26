namespace THEONEEAD.Domain.Repositories;

public interface IUnitOfWorkSeminario
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
