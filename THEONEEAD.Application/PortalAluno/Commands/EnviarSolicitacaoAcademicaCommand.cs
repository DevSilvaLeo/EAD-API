using MediatR;
using THEONEEAD.Application.PortalAluno;

namespace THEONEEAD.Application.PortalAluno.Commands;

public record EnviarSolicitacaoAcademicaCommand(SolicitacaoAcademicaRequestDto Request) : IRequest<SolicitacaoAcademicaResponseDto>;
