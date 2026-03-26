using THEONEEAD.Domain.Common;
using THEONEEAD.Domain.Enums;

namespace THEONEEAD.Domain.Entities;

public class Conteudo : EntityBase
{
    public Guid DisciplinaEADId { get; private set; }
    public string Titulo { get; private set; } = null!;
    public TipoConteudo Tipo { get; private set; }
    public string? UrlVideo { get; private set; }
    public string? UrlSlides { get; private set; }
    public string? ConteudoTexto { get; private set; }
    public int Ordem { get; private set; }
    public int? DuracaoSegundos { get; private set; }

    public DisciplinaEAD? Disciplina { get; private set; }

    private Conteudo() { }

    internal static Conteudo Criar(
        Guid disciplinaEADId,
        string titulo,
        TipoConteudo tipo,
        int ordem,
        string? urlVideo,
        string? urlSlides,
        string? conteudoTexto,
        int? duracaoSegundos)
    {
        if (disciplinaEADId == Guid.Empty)
            throw new ArgumentException("Disciplina inválida.", nameof(disciplinaEADId));
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("Título é obrigatório.", nameof(titulo));

        ValidarCamposPorTipo(tipo, urlVideo, urlSlides, conteudoTexto);

        return new Conteudo
        {
            DisciplinaEADId = disciplinaEADId,
            Titulo = titulo.Trim(),
            Tipo = tipo,
            Ordem = ordem,
            UrlVideo = string.IsNullOrWhiteSpace(urlVideo) ? null : urlVideo.Trim(),
            UrlSlides = string.IsNullOrWhiteSpace(urlSlides) ? null : urlSlides.Trim(),
            ConteudoTexto = string.IsNullOrWhiteSpace(conteudoTexto) ? null : conteudoTexto.Trim(),
            DuracaoSegundos = duracaoSegundos
        };
    }

    public void Atualizar(
        string titulo,
        TipoConteudo tipo,
        int ordem,
        string? urlVideo,
        string? urlSlides,
        string? conteudoTexto,
        int? duracaoSegundos)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("Título é obrigatório.", nameof(titulo));
        ValidarCamposPorTipo(tipo, urlVideo, urlSlides, conteudoTexto);

        Titulo = titulo.Trim();
        Tipo = tipo;
        Ordem = ordem;
        UrlVideo = string.IsNullOrWhiteSpace(urlVideo) ? null : urlVideo.Trim();
        UrlSlides = string.IsNullOrWhiteSpace(urlSlides) ? null : urlSlides.Trim();
        ConteudoTexto = string.IsNullOrWhiteSpace(conteudoTexto) ? null : conteudoTexto.Trim();
        DuracaoSegundos = duracaoSegundos;
    }

    private static void ValidarCamposPorTipo(TipoConteudo tipo, string? urlVideo, string? urlSlides, string? conteudoTexto)
    {
        switch (tipo)
        {
            case TipoConteudo.Video:
                if (string.IsNullOrWhiteSpace(urlVideo))
                    throw new ArgumentException("Url do vídeo é obrigatória para tipo vídeo.");
                break;
            case TipoConteudo.Slide:
                if (string.IsNullOrWhiteSpace(urlSlides))
                    throw new ArgumentException("Url dos slides é obrigatória para tipo slide.");
                break;
            case TipoConteudo.Texto:
                if (string.IsNullOrWhiteSpace(conteudoTexto))
                    throw new ArgumentException("Texto é obrigatório para tipo texto.");
                break;
            case TipoConteudo.Tarefa:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tipo));
        }
    }
}
