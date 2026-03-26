using THEONEEAD.Domain.Common;

namespace THEONEEAD.Domain.Entities;

public class DisciplinaEAD : EntityBase
{
    public Guid CursoEADId { get; private set; }
    public string Nome { get; private set; } = null!;
    public int Ordem { get; private set; }

    public CursoEAD? Curso { get; private set; }

    public List<Conteudo> Conteudos { get; private set; } = new();

    private DisciplinaEAD() { }

    internal static DisciplinaEAD Criar(Guid cursoEADId, string nome, int ordem)
    {
        if (cursoEADId == Guid.Empty)
            throw new ArgumentException("Curso EAD inválido.", nameof(cursoEADId));
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.", nameof(nome));

        return new DisciplinaEAD
        {
            CursoEADId = cursoEADId,
            Nome = nome.Trim(),
            Ordem = ordem
        };
    }

    public void Renomear(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.", nameof(nome));
        Nome = nome.Trim();
    }

    public void Reordenar(int ordem) => Ordem = ordem;

    public Conteudo AdicionarConteudo(
        string titulo,
        Enums.TipoConteudo tipo,
        int ordem,
        string? urlVideo,
        string? urlSlides,
        string? conteudoTexto,
        int? duracaoSegundos)
    {
        var conteudo = Conteudo.Criar(Id, titulo, tipo, ordem, urlVideo, urlSlides, conteudoTexto, duracaoSegundos);
        Conteudos.Add(conteudo);
        return conteudo;
    }
}
