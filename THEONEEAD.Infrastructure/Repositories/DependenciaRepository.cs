using Microsoft.EntityFrameworkCore;
using THEONEEAD.Domain.Entities;
using THEONEEAD.Domain.Enums;
using THEONEEAD.Domain.Repositories;
using THEONEEAD.Infrastructure.Persistence.Seminario;

namespace THEONEEAD.Infrastructure.Repositories;

public class DependenciaRepository : IDependenciaRepository
{
    private readonly SeminarioDbContext _ctx;

    public DependenciaRepository(SeminarioDbContext ctx) => _ctx = ctx;

    public void Adicionar(Dependencia dependencia) => _ctx.Dependencias.Add(dependencia);

    public Task<bool> ExisteAsync(Guid origemId, Guid destinoId, TipoDependencia tipo, CancellationToken cancellationToken = default) =>
        _ctx.Dependencias.AnyAsync(d => d.OrigemId == origemId && d.DestinoId == destinoId && d.Tipo == tipo, cancellationToken);

    public async Task<IReadOnlyList<Dependencia>> ListarPorDestinosConteudoAsync(IEnumerable<Guid> conteudoIds, CancellationToken cancellationToken = default)
    {
        var set = conteudoIds.ToHashSet();
        return await _ctx.Dependencias.AsNoTracking()
            .Where(d => d.Tipo == TipoDependencia.Conteudo && set.Contains(d.DestinoId))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Dependencia>> ListarPorDestinosDisciplinaAsync(IEnumerable<Guid> disciplinaIds, CancellationToken cancellationToken = default)
    {
        var set = disciplinaIds.ToHashSet();
        return await _ctx.Dependencias.AsNoTracking()
            .Where(d => d.Tipo == TipoDependencia.Disciplina && set.Contains(d.DestinoId))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Dependencia>> ListarTodasDoCursoAsync(Guid cursoEADId, CancellationToken cancellationToken = default)
    {
        var conteudoDestinoIds = await _ctx.Conteudos.AsNoTracking()
            .Join(
                _ctx.DisciplinasEAD.AsNoTracking().Where(d => d.CursoEADId == cursoEADId),
                c => c.DisciplinaEADId,
                d => d.Id,
                (c, _) => c.Id)
            .ToListAsync(cancellationToken);

        var disciplinaDestinoIds = await _ctx.DisciplinasEAD.AsNoTracking()
            .Where(d => d.CursoEADId == cursoEADId)
            .Select(d => d.Id)
            .ToListAsync(cancellationToken);

        var cSet = conteudoDestinoIds.ToHashSet();
        var dSet = disciplinaDestinoIds.ToHashSet();

        return await _ctx.Dependencias.AsNoTracking()
            .Where(dep =>
                (dep.Tipo == TipoDependencia.Conteudo && cSet.Contains(dep.DestinoId)) ||
                (dep.Tipo == TipoDependencia.Disciplina && dSet.Contains(dep.DestinoId)))
            .ToListAsync(cancellationToken);
    }
}
