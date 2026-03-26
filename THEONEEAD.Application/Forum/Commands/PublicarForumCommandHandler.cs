using MediatR;
using THEONEEAD.Application.Auth.Dtos;
using THEONEEAD.Domain.Enums;
using THEONEEAD.Domain.Forum;
using THEONEEAD.Domain.Repositories;

namespace THEONEEAD.Application.Forum.Commands;

public class PublicarForumCommandHandler : IRequestHandler<PublicarForumCommand, MensagemSimplesResponseDto>
{
    private readonly IForumRepository _forum;

    public PublicarForumCommandHandler(IForumRepository forum) => _forum = forum;

    public async Task<MensagemSimplesResponseDto> Handle(PublicarForumCommand request, CancellationToken cancellationToken)
    {
        var tipo = ParseTipo(request.TipoRota);
        var id = ForumThread.MontarId(tipo, request.ReferenciaId);

        var thread = await _forum.ObterPorReferenciaAsync(tipo, request.ReferenciaId, cancellationToken);
        if (thread is null)
        {
            thread = new ForumThread
            {
                Id = id,
                Tipo = tipo,
                ReferenciaId = request.ReferenciaId,
                Entradas = new List<ForumEntrada>()
            };
        }

        if (string.IsNullOrWhiteSpace(request.ParentEntradaId))
        {
            thread.Entradas.Add(new ForumEntrada
            {
                AlunoId = request.AlunoIdAutor,
                Mensagem = request.Mensagem.Trim(),
                CriadoEm = DateTime.UtcNow
            });
        }
        else
        {
            var entrada = thread.Entradas.FirstOrDefault(e => e.Id == request.ParentEntradaId)
                ?? throw new InvalidOperationException("Mensagem pai não encontrada.");
            entrada.Respostas.Add(new ForumResposta
            {
                AlunoId = request.AlunoIdAutor,
                Mensagem = request.Mensagem.Trim(),
                CriadoEm = DateTime.UtcNow
            });
        }

        await _forum.SalvarAsync(thread, cancellationToken);
        return new MensagemSimplesResponseDto("Publicado.");
    }

    private static ForumTipoReferencia ParseTipo(string tipo)
    {
        var t = tipo.Trim().ToLowerInvariant();
        return t switch
        {
            "disciplina" => ForumTipoReferencia.Disciplina,
            "conteudo" => ForumTipoReferencia.Conteudo,
            _ => throw new ArgumentException("Tipo de fórum inválido.")
        };
    }
}
