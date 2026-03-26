using THEONEEAD.Domain.Entities;
using THEONEEAD.Domain.Enums;

namespace THEONEEAD.Domain.Repositories;

public interface IDependenciaRepository
{
    Task<IReadOnlyList<Dependencia>> ListarPorDestinosConteudoAsync(IEnumerable<Guid> conteudoIds, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Dependencia>> ListarPorDestinosDisciplinaAsync(IEnumerable<Guid> disciplinaIds, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Dependencia>> ListarTodasDoCursoAsync(Guid cursoEADId, CancellationToken cancellationToken = default);
    void Adicionar(Dependencia dependencia);
    Task<bool> ExisteAsync(Guid origemId, Guid destinoId, TipoDependencia tipo, CancellationToken cancellationToken = default);
}
