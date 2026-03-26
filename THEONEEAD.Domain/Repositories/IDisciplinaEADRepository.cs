using THEONEEAD.Domain.Entities;

namespace THEONEEAD.Domain.Repositories;

public interface IDisciplinaEADRepository
{
    Task<DisciplinaEAD?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DisciplinaEAD?> ObterPorIdComConteudosAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DisciplinaEAD>> ListarPorCursoIdAsync(Guid cursoEADId, CancellationToken cancellationToken = default);
}
