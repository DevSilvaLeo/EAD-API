using THEONEEAD.Domain.Entities;

namespace THEONEEAD.Domain.Repositories;

public interface IConteudoRepository
{
    Task<Conteudo?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Conteudo?> ObterPorIdComDisciplinaAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Conteudo>> ListarPorDisciplinaIdAsync(Guid disciplinaId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Conteudo>> ListarPorCursoIdAsync(Guid cursoEADId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Conteudo>> ListarTarefasPorCursoIdsAsync(IEnumerable<Guid> cursoEADIds, CancellationToken cancellationToken = default);
}
