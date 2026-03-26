using MediatR;
using THEONEEAD.Application.Auth.Dtos;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Application.Cursos;
using THEONEEAD.Domain.Enums;
using THEONEEAD.Domain.Exceptions;
using THEONEEAD.Domain.Repositories;
using THEONEEAD.Domain.Services;

namespace THEONEEAD.Application.Cursos.Commands;

public class ConcluirConteudoCommandHandler : IRequestHandler<ConcluirConteudoCommand, MensagemSimplesResponseDto>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IAlunoCursoReadRepository _alunoCursos;
    private readonly ICursoEADRepository _cursos;
    private readonly IConteudoRepository _conteudos;
    private readonly IAlunoCursoProgressoRepository _progresso;
    private readonly IDependenciaRepository _dependencias;
    private readonly IRegraAcessoConteudoDomainService _regraAcesso;

    public ConcluirConteudoCommandHandler(
        ICurrentUserService currentUser,
        IAlunoCursoReadRepository alunoCursos,
        ICursoEADRepository cursos,
        IConteudoRepository conteudos,
        IAlunoCursoProgressoRepository progresso,
        IDependenciaRepository dependencias,
        IRegraAcessoConteudoDomainService regraAcesso)
    {
        _currentUser = currentUser;
        _alunoCursos = alunoCursos;
        _cursos = cursos;
        _conteudos = conteudos;
        _progresso = progresso;
        _dependencias = dependencias;
        _regraAcesso = regraAcesso;
    }

    public async Task<MensagemSimplesResponseDto> Handle(ConcluirConteudoCommand request, CancellationToken cancellationToken)
    {
        var alunoId = _currentUser.AlunoLegadoId
            ?? throw new UnauthorizedAccessException("Aluno não identificado.");

        var curso = await _cursos.ObterPorIdComDisciplinasEConteudosAsync(request.CursoId, cancellationToken)
            ?? throw new InvalidOperationException("Curso não encontrado.");

        var permitidos = await _alunoCursos.ObterIdsCursosDoAlunoAsync(alunoId, cancellationToken);
        if (!permitidos.Contains(curso.CursoLegadoId))
            throw new UnauthorizedAccessException("Curso não disponível.");

        var conteudo = await _conteudos.ObterPorIdComDisciplinaAsync(request.ConteudoId, cancellationToken)
            ?? throw new InvalidOperationException("Conteúdo não encontrado.");

        if (conteudo.Disciplina is null || conteudo.Disciplina.CursoEADId != curso.Id)
            throw new InvalidOperationException("Conteúdo não pertence ao curso informado.");

        var doc = await _progresso.ObterAsync(alunoId, curso.Id, cancellationToken);
        if (doc is null)
            doc = ProgressoCursoHelper.GarantirDocumento(alunoId, curso);
        else
            ProgressoCursoHelper.SincronizarEstrutura(doc, curso);

        var deps = await _dependencias.ListarTodasDoCursoAsync(curso.Id, cancellationToken);
        var conteudosConcluidos = ProgressoCursoHelper.ConteudosConcluidos(doc);
        var disciplinasConcluidas = ProgressoCursoHelper.DisciplinasConcluidas(curso, doc);

        if (!_regraAcesso.PodeAcessar(conteudo, deps, conteudosConcluidos, disciplinasConcluidas))
            throw new AcessoConteudoNegadoException("Conclua as dependências antes de acessar este conteúdo.");

        if (conteudo.Tipo == TipoConteudo.Tarefa)
        {
            // entrega obrigatória antes de concluir — verificado em fluxo de tarefa
            throw new InvalidOperationException("Use a rota de entrega de tarefa para concluir este conteúdo.");
        }

        var pd = doc.Disciplinas.FirstOrDefault(x => x.DisciplinaId == conteudo.DisciplinaEADId)
            ?? throw new InvalidOperationException("Progresso inconsistente.");
        var pc = pd.Conteudos.FirstOrDefault(x => x.ConteudoId == conteudo.Id)
            ?? throw new InvalidOperationException("Progresso inconsistente.");

        pc.Concluido = true;
        pc.DataConclusao = DateTime.UtcNow;

        var disc = curso.Disciplinas.First(d => d.Id == conteudo.DisciplinaEADId);
        ProgressoCursoHelper.RecalcularPercentualDisciplina(pd, disc);

        await _progresso.SubstituirAsync(doc, cancellationToken);

        return new MensagemSimplesResponseDto("Conteúdo concluído.");
    }
}
