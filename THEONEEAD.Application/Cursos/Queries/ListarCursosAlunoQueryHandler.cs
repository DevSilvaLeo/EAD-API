using MediatR;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Application.Cursos.Dtos;
using THEONEEAD.Domain.Repositories;

namespace THEONEEAD.Application.Cursos.Queries;

public class ListarCursosAlunoQueryHandler : IRequestHandler<ListarCursosAlunoQuery, IReadOnlyList<CursoListaFrontendItemDto>>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IAlunoCursoReadRepository _alunoCursos;
    private readonly ICursoEADRepository _cursos;
    private readonly IAlunoCursoProgressoRepository _progresso;

    public ListarCursosAlunoQueryHandler(
        ICurrentUserService currentUser,
        IAlunoCursoReadRepository alunoCursos,
        ICursoEADRepository cursos,
        IAlunoCursoProgressoRepository progresso)
    {
        _currentUser = currentUser;
        _alunoCursos = alunoCursos;
        _cursos = cursos;
        _progresso = progresso;
    }

    public async Task<IReadOnlyList<CursoListaFrontendItemDto>> Handle(ListarCursosAlunoQuery request, CancellationToken cancellationToken)
    {
        var alunoId = _currentUser.AlunoLegadoId
            ?? throw new UnauthorizedAccessException("Aluno não identificado.");

        var cursoLegadoIds = await _alunoCursos.ObterIdsCursosDoAlunoAsync(alunoId, cancellationToken);
        if (cursoLegadoIds.Count == 0)
            return Array.Empty<CursoListaFrontendItemDto>();

        var cursosEad = await _cursos.ListarPorCursoLegadoIdsAsync(cursoLegadoIds, cancellationToken);
        var lista = new List<CursoListaFrontendItemDto>();

        foreach (var c in cursosEad.OrderBy(x => x.Nome))
        {
            var doc = await _progresso.ObterAsync(alunoId, c.Id, cancellationToken);
            decimal pct = 0;
            if (doc is not null && doc.Disciplinas.Count > 0)
                pct = Math.Round(doc.Disciplinas.Sum(d => d.Percentual) / doc.Disciplinas.Count, 2);

            lista.Add(new CursoListaFrontendItemDto(c.Id.ToString("D"), c.Nome, pct));
        }

        return lista;
    }
}
