using Microsoft.EntityFrameworkCore;
using THEONEEAD.Domain.Entities;
using THEONEEAD.Domain.Enums;
using THEONEEAD.Domain.Repositories;
using THEONEEAD.Infrastructure.Persistence.Seminario;

namespace THEONEEAD.Infrastructure.Repositories;

public class ConteudoRepository : IConteudoRepository
{
    private readonly SeminarioDbContext _ctx;

    public ConteudoRepository(SeminarioDbContext ctx) => _ctx = ctx;

    public Task<Conteudo?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _ctx.Conteudos.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Conteudo?> ObterPorIdComDisciplinaAsync(Guid id, CancellationToken cancellationToken = default) =>
        _ctx.Conteudos.Include(c => c.Disciplina).FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Conteudo>> ListarPorDisciplinaIdAsync(Guid disciplinaId, CancellationToken cancellationToken = default)
    {
        var list = await _ctx.Conteudos.AsNoTracking()
            .Where(c => c.DisciplinaEADId == disciplinaId)
            .OrderBy(c => c.Ordem)
            .ToListAsync(cancellationToken);
        return list;
    }

    public async Task<IReadOnlyList<Conteudo>> ListarPorCursoIdAsync(Guid cursoEADId, CancellationToken cancellationToken = default)
    {
        var list = await _ctx.Conteudos.AsNoTracking()
            .Include(c => c.Disciplina)
            .Where(c => c.Disciplina!.CursoEADId == cursoEADId)
            .OrderBy(c => c.Ordem)
            .ToListAsync(cancellationToken);
        return list;
    }

    public async Task<IReadOnlyList<Conteudo>> ListarTarefasPorCursoIdsAsync(IEnumerable<Guid> cursoEADIds, CancellationToken cancellationToken = default)
    {
        var ids = cursoEADIds.ToHashSet();
        var list = await _ctx.Conteudos.AsNoTracking()
            .Include(c => c.Disciplina)
            .Where(c => c.Tipo == TipoConteudo.Tarefa && c.Disciplina != null && ids.Contains(c.Disciplina.CursoEADId))
            .OrderBy(c => c.Ordem)
            .ToListAsync(cancellationToken);
        return list;
    }
}
