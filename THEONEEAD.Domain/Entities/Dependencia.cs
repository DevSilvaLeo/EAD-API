using THEONEEAD.Domain.Common;
using THEONEEAD.Domain.Enums;

namespace THEONEEAD.Domain.Entities;

/// <summary>
/// Destino depende de Origem: para liberar o destino, a origem deve estar concluída.
/// </summary>
public class Dependencia : EntityBase
{
    public Guid OrigemId { get; private set; }
    public Guid DestinoId { get; private set; }
    public TipoDependencia Tipo { get; private set; }

    private Dependencia() { }

    public static Dependencia EntreConteudos(Guid origemConteudoId, Guid destinoConteudoId)
    {
        ValidarPar(origemConteudoId, destinoConteudoId);
        return new Dependencia
        {
            OrigemId = origemConteudoId,
            DestinoId = destinoConteudoId,
            Tipo = TipoDependencia.Conteudo
        };
    }

    public static Dependencia EntreDisciplinas(Guid origemDisciplinaId, Guid destinoDisciplinaId)
    {
        ValidarPar(origemDisciplinaId, destinoDisciplinaId);
        return new Dependencia
        {
            OrigemId = origemDisciplinaId,
            DestinoId = destinoDisciplinaId,
            Tipo = TipoDependencia.Disciplina
        };
    }

    private static void ValidarPar(Guid origem, Guid destino)
    {
        if (origem == Guid.Empty || destino == Guid.Empty)
            throw new ArgumentException("Identificadores inválidos.");
        if (origem == destino)
            throw new ArgumentException("Origem e destino não podem ser iguais.");
    }
}
