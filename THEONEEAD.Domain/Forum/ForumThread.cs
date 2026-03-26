using THEONEEAD.Domain.Enums;

namespace THEONEEAD.Domain.Forum;

/// <summary>
/// Tópico de fórum por referência (disciplina ou conteúdo), com várias entradas.
/// </summary>
public class ForumThread
{
    public string Id { get; set; } = null!;
    public ForumTipoReferencia Tipo { get; set; }
    public string ReferenciaId { get; set; } = null!;
    public List<ForumEntrada> Entradas { get; set; } = new();

    public static string MontarId(ForumTipoReferencia tipo, string referenciaId) => $"{(int)tipo}:{referenciaId}";
}
