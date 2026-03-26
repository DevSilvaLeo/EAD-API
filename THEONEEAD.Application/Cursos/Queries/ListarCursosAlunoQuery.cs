using MediatR;
using THEONEEAD.Application.Cursos.Dtos;

namespace THEONEEAD.Application.Cursos.Queries;

public record ListarCursosAlunoQuery : IRequest<IReadOnlyList<CursoListaFrontendItemDto>>;
