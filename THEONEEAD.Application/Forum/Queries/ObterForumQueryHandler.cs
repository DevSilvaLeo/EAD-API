using MediatR;
using THEONEEAD.Application.Forum.Dtos;
using THEONEEAD.Domain.Enums;
using THEONEEAD.Domain.Forum;
using THEONEEAD.Domain.Repositories;

namespace THEONEEAD.Application.Forum.Queries;

public class ObterForumQueryHandler : IRequestHandler<ObterForumQuery, ForumThreadResponseDto?>
{
    private readonly IForumRepository _forum;

    public ObterForumQueryHandler(IForumRepository forum) => _forum = forum;

    public async Task<ForumThreadResponseDto?> Handle(ObterForumQuery request, CancellationToken cancellationToken)
    {
        var tipo = ParseTipo(request.TipoRota);
        var thread = await _forum.ObterPorReferenciaAsync(tipo, request.ReferenciaId, cancellationToken);
        if (thread is null)
            return new ForumThreadResponseDto(
                ForumThread.MontarId(tipo, request.ReferenciaId),
                request.TipoRota.ToLowerInvariant(),
                request.ReferenciaId,
                Array.Empty<ForumEntradaDto>());

        return Mapear(thread, request.TipoRota);
    }

    private static ForumTipoReferencia ParseTipo(string tipo)
    {
        var t = tipo.Trim().ToLowerInvariant();
        return t switch
        {
            "disciplina" => ForumTipoReferencia.Disciplina,
            "conteudo" => ForumTipoReferencia.Conteudo,
            _ => throw new ArgumentException("Tipo de fórum inválido. Use disciplina ou conteudo.")
        };
    }

    private static ForumThreadResponseDto Mapear(ForumThread thread, string tipoRota)
    {
        var entradas = thread.Entradas
            .OrderBy(e => e.CriadoEm)
            .Select(e => new ForumEntradaDto(
                e.Id,
                e.AlunoId,
                e.Mensagem,
                e.CriadoEm,
                e.Respostas.OrderBy(r => r.CriadoEm).Select(r => new ForumRespostaDto(r.Id, r.AlunoId, r.Mensagem, r.CriadoEm)).ToList()))
            .ToList();

        return new ForumThreadResponseDto(thread.Id, tipoRota.ToLowerInvariant(), thread.ReferenciaId, entradas);
    }
}
