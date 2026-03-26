using MediatR;
using THEONEEAD.Application.Admin.Dtos;
using THEONEEAD.Domain.Enums;
using THEONEEAD.Domain.Repositories;

namespace THEONEEAD.Application.Admin.Commands;

public class CriarConteudoCommandHandler : IRequestHandler<CriarConteudoCommand, RecursoCriadoResponseDto>
{
    private readonly IDisciplinaEADRepository _disciplinas;
    private readonly IUnitOfWorkSeminario _uow;

    public CriarConteudoCommandHandler(IDisciplinaEADRepository disciplinas, IUnitOfWorkSeminario uow)
    {
        _disciplinas = disciplinas;
        _uow = uow;
    }

    public async Task<RecursoCriadoResponseDto> Handle(CriarConteudoCommand request, CancellationToken cancellationToken)
    {
        var disciplina = await _disciplinas.ObterPorIdComConteudosAsync(request.DisciplinaEADId, cancellationToken)
            ?? throw new InvalidOperationException("Disciplina não encontrada.");

        var tipo = ParseTipo(request.Tipo);
        var conteudo = disciplina.AdicionarConteudo(
            request.Titulo,
            tipo,
            request.Ordem,
            request.UrlVideo,
            request.UrlSlides,
            request.ConteudoTexto,
            request.DuracaoSegundos);

        await _uow.SaveChangesAsync(cancellationToken);
        return new RecursoCriadoResponseDto(conteudo.Id);
    }

    private static TipoConteudo ParseTipo(string tipo)
    {
        return tipo.Trim().ToLowerInvariant() switch
        {
            "video" => TipoConteudo.Video,
            "slide" or "slides" => TipoConteudo.Slide,
            "texto" => TipoConteudo.Texto,
            "tarefa" => TipoConteudo.Tarefa,
            _ => throw new ArgumentException("Tipo de conteúdo inválido.")
        };
    }
}
