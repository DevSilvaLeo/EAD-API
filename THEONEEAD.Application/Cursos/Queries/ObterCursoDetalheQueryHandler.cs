using MediatR;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Application.Cursos;
using THEONEEAD.Application.Cursos.Dtos;
using THEONEEAD.Domain.Enums;
using THEONEEAD.Domain.Repositories;
using THEONEEAD.Domain.Services;

namespace THEONEEAD.Application.Cursos.Queries;

public class ObterCursoDetalheQueryHandler : IRequestHandler<ObterCursoDetalheQuery, CursoDetalheFrontendDto?>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IAlunoCursoReadRepository _alunoCursos;
    private readonly ICursoEADRepository _cursos;
    private readonly IAlunoCursoProgressoRepository _progresso;
    private readonly IDependenciaRepository _dependencias;
    private readonly IRegraAcessoConteudoDomainService _regraAcesso;

    public ObterCursoDetalheQueryHandler(
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

    public async Task<CursoDetalheFrontendDto?> Handle(ObterCursoDetalheQuery request, CancellationToken cancellationToken)
    {
        var alunoId = _currentUser.AlunoLegadoId
            ?? throw new UnauthorizedAccessException("Aluno não identificado.");

        var curso = await _cursos.ObterPorIdComDisciplinasEConteudosAsync(request.CursoId, cancellationToken);
        if (curso is null)
            return null;

        var permitidos = await _alunoCursos.ObterIdsCursosDoAlunoAsync(alunoId, cancellationToken);
        if (!permitidos.Contains(curso.CursoLegadoId))
            throw new UnauthorizedAccessException("Curso não disponível para este aluno.");

        var doc = await _progresso.ObterAsync(alunoId, curso.Id, cancellationToken);
        if (doc is null)
            doc = ProgressoCursoHelper.GarantirDocumento(alunoId, curso);
        else
            ProgressoCursoHelper.SincronizarEstrutura(doc, curso);

        await _progresso.SubstituirAsync(doc, cancellationToken);

        var deps = await _dependencias.ListarTodasDoCursoAsync(curso.Id, cancellationToken);
        var conteudosConcluidos = ProgressoCursoHelper.ConteudosConcluidos(doc);
        var disciplinasConcluidas = ProgressoCursoHelper.DisciplinasConcluidas(curso, doc);

        var aulas = new List<AulaFrontendDto>();
        var ordemGlobal = 0;

        foreach (var d in curso.Disciplinas.OrderBy(x => x.Ordem))
        {
            var pd = doc.Disciplinas.First(x => x.DisciplinaId == d.Id);
            foreach (var c in d.Conteudos.OrderBy(x => x.Ordem))
            {
                ordemGlobal++;
                var pc = pd.Conteudos.FirstOrDefault(x => x.ConteudoId == c.Id);
                var concluida = pc?.Concluido ?? false;
                var pendentes = _regraAcesso.ObterDependenciasNaoSatisfeitas(c, deps, conteudosConcluidos, disciplinasConcluidas);
                var pode = pendentes.Count == 0;

                var tipoApi = CursoTipoFrontendMapper.ParaTipoAula(c.Tipo);
                var texto = CursoTipoFrontendMapper.TextoParaTarefaSeNecessario(c.Tipo, c.ConteudoTexto);

                if (curso.Sequencial && !pode)
                {
                    // Mantém tipo para UI; frontend usa sequencial + ordem para bloquear — percentual 0
                }

                aulas.Add(new AulaFrontendDto(
                    c.Id.ToString("D"),
                    c.Titulo,
                    tipoApi,
                    c.Tipo == TipoConteudo.Video ? c.UrlVideo : null,
                    c.Tipo == TipoConteudo.Slide ? c.UrlSlides : null,
                    c.Tipo == TipoConteudo.Texto || c.Tipo == TipoConteudo.Tarefa ? texto : null,
                    ordemGlobal,
                    concluida,
                    concluida ? 100m : 0m,
                    c.DuracaoSegundos));
            }
        }

        var pctGeral = ProgressoCursoHelper.PercentualCurso(curso, doc);

        return new CursoDetalheFrontendDto(
            curso.Id.ToString("D"),
            curso.Nome,
            pctGeral,
            curso.Sequencial,
            aulas);
    }
}
