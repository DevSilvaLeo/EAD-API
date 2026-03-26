using MediatR;
using THEONEEAD.Application.Auth.Dtos;

namespace THEONEEAD.Application.Forum.Commands;

public record PublicarForumCommand(string TipoRota, string ReferenciaId, string Mensagem, string? ParentEntradaId, string AlunoIdAutor)
    : IRequest<MensagemSimplesResponseDto>;
