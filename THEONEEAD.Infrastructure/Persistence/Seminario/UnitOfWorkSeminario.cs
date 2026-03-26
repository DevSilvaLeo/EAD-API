using THEONEEAD.Domain.Repositories;

namespace THEONEEAD.Infrastructure.Persistence.Seminario;

public class UnitOfWorkSeminario : IUnitOfWorkSeminario, IDisposable
{
    private readonly SeminarioDbContext _ctx;

    public UnitOfWorkSeminario(SeminarioDbContext ctx) => _ctx = ctx;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _ctx.SaveChangesAsync(cancellationToken);

    public void Dispose() => _ctx.Dispose();
}
