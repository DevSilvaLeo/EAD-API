using MediatR;
using THEONEEAD.Application.Auth.Dtos;

namespace THEONEEAD.Application.Cursos.Commands;

public record ConcluirConteudoCommand(Guid CursoId, Guid ConteudoId) : IRequest<MensagemSimplesResponseDto>;
