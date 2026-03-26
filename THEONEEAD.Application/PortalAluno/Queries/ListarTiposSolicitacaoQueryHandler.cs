using MediatR;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Application.PortalAluno;

namespace THEONEEAD.Application.PortalAluno.Queries;

public class ListarTiposSolicitacaoQueryHandler : IRequestHandler<ListarTiposSolicitacaoQuery, IReadOnlyList<TipoSolicitacaoDto>>
{
    private readonly IPortalAlunoLeituraRepository _repo;

    public ListarTiposSolicitacaoQueryHandler(IPortalAlunoLeituraRepository repo) => _repo = repo;

    public Task<IReadOnlyList<TipoSolicitacaoDto>> Handle(ListarTiposSolicitacaoQuery request, CancellationToken cancellationToken) =>
        _repo.ListarTiposSolicitacaoAsync(cancellationToken);
}
