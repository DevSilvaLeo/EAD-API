using Microsoft.EntityFrameworkCore;
using THEONEEAD.Domain.Entities;
using THEONEEAD.Domain.Repositories;
using THEONEEAD.Infrastructure.Persistence.Seminario;

namespace THEONEEAD.Infrastructure.Repositories;

public class DisciplinaEADRepository : IDisciplinaEADRepository
{
    private readonly SeminarioDbContext _ctx;

    public DisciplinaEADRepository(SeminarioDbContext ctx) => _ctx = ctx;

    public Task<DisciplinaEAD?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _ctx.DisciplinasEAD.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<DisciplinaEAD?> ObterPorIdComConteudosAsync(Guid id, CancellationToken cancellationToken = default) =>
        _ctx.DisciplinasEAD.Include(d => d.Conteudos).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<DisciplinaEAD>> ListarPorCursoIdAsync(Guid cursoEADId, CancellationToken cancellationToken = default)
    {
        var list = await _ctx.DisciplinasEAD.AsNoTracking()
            .Where(x => x.CursoEADId == cursoEADId)
            .OrderBy(x => x.Ordem)
            .ToListAsync(cancellationToken);
        return list;
    }
}
