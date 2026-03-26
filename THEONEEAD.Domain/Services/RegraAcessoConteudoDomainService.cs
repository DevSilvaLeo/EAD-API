using THEONEEAD.Domain.Entities;
using THEONEEAD.Domain.Enums;

namespace THEONEEAD.Domain.Services;

public class RegraAcessoConteudoDomainService : IRegraAcessoConteudoDomainService
{
    public bool PodeAcessar(
        Conteudo conteudo,
        IReadOnlyCollection<Dependencia> dependencias,
        IReadOnlySet<Guid> conteudosConcluidos,
        IReadOnlySet<Guid> disciplinasConcluidas) =>
        ObterDependenciasNaoSatisfeitas(conteudo, dependencias, conteudosConcluidos, disciplinasConcluidas).Count == 0;

    public IReadOnlyList<Guid> ObterDependenciasNaoSatisfeitas(
        Conteudo conteudo,
        IReadOnlyCollection<Dependencia> dependencias,
        IReadOnlySet<Guid> conteudosConcluidos,
        IReadOnlySet<Guid> disciplinasConcluidas)
    {
        var pendentes = new List<Guid>();
        foreach (var dep in dependencias.Where(d => d.DestinoId == conteudo.Id && d.Tipo == TipoDependencia.Conteudo))
        {
            if (!conteudosConcluidos.Contains(dep.OrigemId))
                pendentes.Add(dep.OrigemId);
        }

        foreach (var dep in dependencias.Where(d => d.DestinoId == conteudo.DisciplinaEADId && d.Tipo == TipoDependencia.Disciplina))
        {
            if (!disciplinasConcluidas.Contains(dep.OrigemId))
                pendentes.Add(dep.OrigemId);
        }

        return pendentes;
    }
}
