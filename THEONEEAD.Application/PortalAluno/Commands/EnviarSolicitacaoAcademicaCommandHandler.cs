using MediatR;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Application.PortalAluno;

namespace THEONEEAD.Application.PortalAluno.Commands;

public class EnviarSolicitacaoAcademicaCommandHandler : IRequestHandler<EnviarSolicitacaoAcademicaCommand, SolicitacaoAcademicaResponseDto>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IPortalAlunoLeituraRepository _repo;

    public EnviarSolicitacaoAcademicaCommandHandler(ICurrentUserService currentUser, IPortalAlunoLeituraRepository repo)
    {
        _currentUser = currentUser;
        _repo = repo;
    }

    public async Task<SolicitacaoAcademicaResponseDto> Handle(EnviarSolicitacaoAcademicaCommand request, CancellationToken cancellationToken)
    {
        var alunoId = _currentUser.AlunoLegadoId
            ?? throw new UnauthorizedAccessException("Aluno não identificado.");
        return await _repo.RegistrarSolicitacaoAsync(alunoId, request.Request, cancellationToken);
    }
}
