using MediatR;
using THEONEEAD.Application.Cursos.Dtos;

namespace THEONEEAD.Application.Cursos.Queries;

public record ObterCursoDetalheQuery(Guid CursoId) : IRequest<CursoDetalheFrontendDto?>;
