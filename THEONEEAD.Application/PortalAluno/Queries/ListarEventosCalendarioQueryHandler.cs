using MediatR;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Application.PortalAluno;

namespace THEONEEAD.Application.PortalAluno.Queries;

public class ListarEventosCalendarioQueryHandler : IRequestHandler<ListarEventosCalendarioQuery, IReadOnlyList<EventoCalendarioDto>>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IPortalAlunoLeituraRepository _repo;

    public ListarEventosCalendarioQueryHandler(ICurrentUserService currentUser, IPortalAlunoLeituraRepository repo)
    {
        _currentUser = currentUser;
        _repo = repo;
    }

    public async Task<IReadOnlyList<EventoCalendarioDto>> Handle(ListarEventosCalendarioQuery request, CancellationToken cancellationToken)
    {
        var alunoId = _currentUser.AlunoLegadoId
            ?? throw new UnauthorizedAccessException("Aluno não identificado.");
        return await _repo.ListarEventosCalendarioAsync(alunoId, cancellationToken);
    }
}
