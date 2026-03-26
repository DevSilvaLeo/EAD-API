using MediatR;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Application.PortalAluno;

namespace THEONEEAD.Application.PortalAluno.Queries;

public class ListarAvisosAlunoQueryHandler : IRequestHandler<ListarAvisosAlunoQuery, IReadOnlyList<AvisoDto>>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IPortalAlunoLeituraRepository _repo;

    public ListarAvisosAlunoQueryHandler(ICurrentUserService currentUser, IPortalAlunoLeituraRepository repo)
    {
        _currentUser = currentUser;
        _repo = repo;
    }

    public async Task<IReadOnlyList<AvisoDto>> Handle(ListarAvisosAlunoQuery request, CancellationToken cancellationToken)
    {
        var alunoId = _currentUser.AlunoLegadoId
            ?? throw new UnauthorizedAccessException("Aluno não identificado.");
        return await _repo.ListarAvisosAsync(alunoId, cancellationToken);
    }
}
