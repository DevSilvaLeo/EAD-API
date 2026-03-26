using THEONEEAD.Domain.Progresso;

namespace THEONEEAD.Domain.Repositories;

public interface IAlunoCursoProgressoRepository
{
    Task<AlunoCursoProgresso?> ObterAsync(long alunoLegadoId, Guid cursoEADId, CancellationToken cancellationToken = default);
    Task SubstituirAsync(AlunoCursoProgresso documento, CancellationToken cancellationToken = default);
}
