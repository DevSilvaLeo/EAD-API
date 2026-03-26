using MediatR;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Application.Cursos;
using THEONEEAD.Application.Tarefas.Dtos;
using THEONEEAD.Domain.Enums;
using THEONEEAD.Domain.Repositories;
using THEONEEAD.Domain.Services;

namespace THEONEEAD.Application.Tarefas.Queries;

public class ListarTarefasPendentesQueryHandler : IRequestHandler<ListarTarefasPendentesQuery, IReadOnlyList<TarefaPendenteFrontendDto>>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IAlunoCursoReadRepository _alunoCursos;
    private readonly ICursoEADRepository _cursos;
    private readonly IAlunoCursoProgressoRepository _progresso;
    private readonly IDependenciaRepository _dependencias;
    private readonly IRegraAcessoConteudoDomainService _regraAcesso;

    public ListarTarefasPendentesQueryHandler(
        ICurrentUserService currentUser,
        IAlunoCursoReadRepository alunoCursos,
        ICursoEADRepository cursos,
        IAlunoCursoProgressoRepository progresso,
        IDependenciaRepository dependencias,
        IRegraAcessoConteudoDomainService regraAcesso)
    {
        _currentUser = currentUser;
        _alunoCursos = alunoCursos;
        _cursos = cursos;
        _progresso = progresso;
        _dependencias = dependencias;
        _regraAcesso = regraAcesso;
    }

    public async Task<IReadOnlyList<TarefaPendenteFrontendDto>> Handle(ListarTarefasPendentesQuery request, CancellationToken cancellationToken)
    {
        var alunoId = _currentUser.AlunoLegadoId
            ?? throw new UnauthorizedAccessException("Aluno não identificado.");

        var cursoLegadoIds = await _alunoCursos.ObterIdsCursosDoAlunoAsync(alunoId, cancellationToken);
        if (cursoLegadoIds.Count == 0)
            return Array.Empty<TarefaPendenteFrontendDto>();

        var cursosEad = await _cursos.ListarPorCursoLegadoIdsAsync(cursoLegadoIds, cancellationToken);
        if (cursosEad.Count == 0)
            return Array.Empty<TarefaPendenteFrontendDto>();

        var resultado = new List<TarefaPendenteFrontendDto>();

        foreach (var curso in cursosEad)
        {
            var cursoFull = await _cursos.ObterPorIdComDisciplinasEConteudosAsync(curso.Id, cancellationToken);
            if (cursoFull is null)
                continue;

            var doc = await _progresso.ObterAsync(alunoId, curso.Id, cancellationToken);
            if (doc is null)
                doc = ProgressoCursoHelper.GarantirDocumento(alunoId, cursoFull);
            else
                ProgressoCursoHelper.SincronizarEstrutura(doc, cursoFull);

            var deps = await _dependencias.ListarTodasDoCursoAsync(curso.Id, cancellationToken);

            foreach (var d in cursoFull.Disciplinas)
            {
                foreach (var c in d.Conteudos.Where(x => x.Tipo == TipoConteudo.Tarefa))
                {
                    var pc = doc.Disciplinas.FirstOrDefault(x => x.DisciplinaId == d.Id)?.Conteudos.FirstOrDefault(x => x.ConteudoId == c.Id);
                    if (pc?.Concluido == true)
                        continue;

                    var conteudosConcluidos = ProgressoCursoHelper.ConteudosConcluidos(doc);
                    var disciplinasConcluidas = ProgressoCursoHelper.DisciplinasConcluidas(cursoFull, doc);
                    if (!_regraAcesso.PodeAcessar(c, deps, conteudosConcluidos, disciplinasConcluidas))
                        continue;

                    var prazo = DateTime.UtcNow.AddDays(7).ToString("yyyy-MM-dd");
                    var descricao = $"Disciplina: {d.Nome}. Curso: {cursoFull.Nome}.";
                    resultado.Add(new TarefaPendenteFrontendDto(
                        c.Id.ToString("D"),
                        c.Titulo,
                        descricao,
                        prazo,
                        false));
                }
            }
        }

        return resultado.OrderBy(x => x.Titulo).ToList();
    }
}
