using Microsoft.EntityFrameworkCore;
using THEONEEAD.Domain.Repositories;
using THEONEEAD.Infrastructure.Persistence.Seminario;

namespace THEONEEAD.Infrastructure.Repositories;

public class AlunoCursoReadRepository : IAlunoCursoReadRepository
{
    private readonly SeminarioDbContext _ctx;

    public AlunoCursoReadRepository(SeminarioDbContext ctx) => _ctx = ctx;

    public Task<bool> AlunoExisteAsync(long alunoId, CancellationToken cancellationToken = default) =>
        _ctx.AlunoCursos.AsNoTracking().AnyAsync(x => x.AlunoId == alunoId, cancellationToken);

    public async Task<IReadOnlyList<long>> ObterIdsCursosDoAlunoAsync(long alunoId, CancellationToken cancellationToken = default)
    {
        var ids = await _ctx.AlunoCursos.AsNoTracking()
            .Where(x => x.AlunoId == alunoId)
            .Select(x => x.CursoId)
            .Distinct()
            .ToListAsync(cancellationToken);
        return ids;
    }
}
