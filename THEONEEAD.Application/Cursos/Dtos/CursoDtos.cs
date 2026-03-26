using THEONEEAD.Domain.Enums;

namespace THEONEEAD.Application.Cursos.Dtos;

/// <summary>GET /api/cursos — contrato do app Angular.</summary>
public record CursoListaFrontendItemDto(string Id, string Nome, decimal PercentualConcluido);

/// <summary>GET /api/cursos/:id — aula = conteúdo EAD (lista plana).</summary>
public record AulaFrontendDto(
    string Id,
    string Titulo,
    string Tipo,
    string? UrlVideo,
    string? UrlSlides,
    string? ConteudoTexto,
    int Ordem,
    bool Concluida,
    decimal PercentualConcluido,
    int? DuracaoSegundos);

public record CursoDetalheFrontendDto(
    string Id,
    string Nome,
    decimal PercentualConcluidoGeral,
    bool Sequencial,
    IReadOnlyList<AulaFrontendDto> Aulas);

public static class CursoTipoFrontendMapper
{
    /// <summary>Mapeia para video | slides | texto (contrato do frontend).</summary>
    public static string ParaTipoAula(TipoConteudo tipo) => tipo switch
    {
        TipoConteudo.Video => "video",
        TipoConteudo.Slide => "slides",
        TipoConteudo.Texto => "texto",
        TipoConteudo.Tarefa => "texto",
        _ => "texto"
    };

    public static string? TextoParaTarefaSeNecessario(TipoConteudo tipo, string? conteudoTexto)
    {
        if (tipo == TipoConteudo.Tarefa)
            return string.IsNullOrWhiteSpace(conteudoTexto)
                ? "<p><strong>Tarefa:</strong> conclua a entrega pela área de tarefas da plataforma.</p>"
                : conteudoTexto;
        return conteudoTexto;
    }
}
