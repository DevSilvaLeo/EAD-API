using MediatR;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Application.PortalAluno;

namespace THEONEEAD.Application.PortalAluno.Queries;

public class ObterMeusDadosSolicitacaoQueryHandler : IRequestHandler<ObterMeusDadosSolicitacaoQuery, DadosAlunoSolicitacaoDto?>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IPortalAlunoLeituraRepository _repo;

    public ObterMeusDadosSolicitacaoQueryHandler(ICurrentUserService currentUser, IPortalAlunoLeituraRepository repo)
    {
        _currentUser = currentUser;
        _repo = repo;
    }

    public async Task<DadosAlunoSolicitacaoDto?> Handle(ObterMeusDadosSolicitacaoQuery request, CancellationToken cancellationToken)
    {
        var alunoId = _currentUser.AlunoLegadoId
            ?? throw new UnauthorizedAccessException("Aluno não identificado.");
        var email = _currentUser.Email ?? "";
        var nome = _currentUser.Nome ?? "";
        return await _repo.ObterDadosAlunoAsync(alunoId, email, nome, cancellationToken);
    }
}
