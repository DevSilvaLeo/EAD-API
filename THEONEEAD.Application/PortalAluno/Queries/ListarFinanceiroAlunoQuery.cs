using MediatR;
using THEONEEAD.Application.PortalAluno;

namespace THEONEEAD.Application.PortalAluno.Queries;

public record ListarFinanceiroAlunoQuery : IRequest<IReadOnlyList<FinanceiroItemDto>>;
