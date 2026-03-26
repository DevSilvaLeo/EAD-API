using MediatR;
using THEONEEAD.Application.Auth.Dtos;

namespace THEONEEAD.Application.Tarefas.Commands;

public record EntregarTarefaCommand(Guid ConteudoId, string? Texto, string? UrlArquivo) : IRequest<MensagemSimplesResponseDto>;
