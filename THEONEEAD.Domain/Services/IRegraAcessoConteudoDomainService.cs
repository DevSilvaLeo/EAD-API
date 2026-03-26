using THEONEEAD.Domain.Entities;

namespace THEONEEAD.Domain.Services;

/// <summary>
/// Verifica se um conteúdo pode ser acessado com base em dependências de conteúdo e disciplina
/// e no progresso informado (IDs concluídos).
/// </summary>
public interface IRegraAcessoConteudoDomainService
{
    bool PodeAcessar(
        Conteudo conteudo,
        IReadOnlyCollection<Dependencia> dependencias,
        IReadOnlySet<Guid> conteudosConcluidos,
        IReadOnlySet<Guid> disciplinasConcluidas);

    IReadOnlyList<Guid> ObterDependenciasNaoSatisfeitas(
        Conteudo conteudo,
        IReadOnlyCollection<Dependencia> dependencias,
        IReadOnlySet<Guid> conteudosConcluidos,
        IReadOnlySet<Guid> disciplinasConcluidas);
}
