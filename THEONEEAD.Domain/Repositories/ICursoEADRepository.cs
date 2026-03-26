using THEONEEAD.Domain.Entities;

namespace THEONEEAD.Domain.Repositories;

public interface ICursoEADRepository
{
    Task<CursoEAD?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CursoEAD?> ObterPorIdComDisciplinasEConteudosAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CursoEAD>> ListarPorCursoLegadoIdsAsync(IEnumerable<long> cursoLegadoIds, CancellationToken cancellationToken = default);
    Task<bool> ExistePorCursoLegadoIdAsync(long cursoLegadoId, CancellationToken cancellationToken = default);
    void Adicionar(CursoEAD curso);
}
