using MediatR;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Application.PortalAluno;

namespace THEONEEAD.Application.PortalAluno.Queries;

public class ListarFinanceiroAlunoQueryHandler : IRequestHandler<ListarFinanceiroAlunoQuery, IReadOnlyList<FinanceiroItemDto>>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IPortalAlunoLeituraRepository _repo;

    public ListarFinanceiroAlunoQueryHandler(ICurrentUserService currentUser, IPortalAlunoLeituraRepository repo)
    {
        _currentUser = currentUser;
        _repo = repo;
    }

    public async Task<IReadOnlyList<FinanceiroItemDto>> Handle(ListarFinanceiroAlunoQuery request, CancellationToken cancellationToken)
    {
        var alunoId = _currentUser.AlunoLegadoId
            ?? throw new UnauthorizedAccessException("Aluno não identificado.");
        return await _repo.ListarFinanceiroAsync(alunoId, cancellationToken);
    }
}
