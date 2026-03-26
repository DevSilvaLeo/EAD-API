using Microsoft.EntityFrameworkCore;
using THEONEEAD.Domain.Entities;
using THEONEEAD.Domain.Repositories;
using THEONEEAD.Infrastructure.Persistence.Seminario;

namespace THEONEEAD.Infrastructure.Repositories;

public class CursoEADRepository : ICursoEADRepository
{
    private readonly SeminarioDbContext _ctx;

    public CursoEADRepository(SeminarioDbContext ctx) => _ctx = ctx;

    public void Adicionar(CursoEAD curso) => _ctx.CursosEAD.Add(curso);

    public Task<bool> ExistePorCursoLegadoIdAsync(long cursoLegadoId, CancellationToken cancellationToken = default) =>
        _ctx.CursosEAD.AnyAsync(x => x.CursoLegadoId == cursoLegadoId, cancellationToken);

    public Task<CursoEAD?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _ctx.CursosEAD.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<CursoEAD?> ObterPorIdComDisciplinasEConteudosAsync(Guid id, CancellationToken cancellationToken = default) =>
        _ctx.CursosEAD
            .Include(c => c.Disciplinas)
            .ThenInclude(d => d.Conteudos)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<CursoEAD>> ListarPorCursoLegadoIdsAsync(IEnumerable<long> cursoLegadoIds, CancellationToken cancellationToken = default)
    {
        var set = cursoLegadoIds.ToHashSet();
        return await _ctx.CursosEAD.AsNoTracking().Where(x => set.Contains(x.CursoLegadoId)).ToListAsync(cancellationToken);
    }
}
