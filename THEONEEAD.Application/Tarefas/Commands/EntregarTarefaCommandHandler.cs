using MediatR;
using THEONEEAD.Application.Auth.Dtos;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Application.Cursos;
using THEONEEAD.Domain.Enums;
using THEONEEAD.Domain.Exceptions;
using THEONEEAD.Domain.Repositories;
using THEONEEAD.Domain.Services;
using THEONEEAD.Domain.Tarefas;

namespace THEONEEAD.Application.Tarefas.Commands;

public class EntregarTarefaCommandHandler : IRequestHandler<EntregarTarefaCommand, MensagemSimplesResponseDto>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IAlunoCursoReadRepository _alunoCursos;
    private readonly IConteudoRepository _conteudos;
    private readonly ICursoEADRepository _cursos;
    private readonly IAlunoCursoProgressoRepository _progresso;
    private readonly IDependenciaRepository _dependencias;
    private readonly IRegraAcessoConteudoDomainService _regraAcesso;
    private readonly ITarefaEntregaRepository _entregas;

    public EntregarTarefaCommandHandler(
        ICurrentUserService currentUser,
        IAlunoCursoReadRepository alunoCursos,
        IConteudoRepository conteudos,
        ICursoEADRepository cursos,
        IAlunoCursoProgressoRepository progresso,
        IDependenciaRepository dependencias,
        IRegraAcessoConteudoDomainService regraAcesso,
        ITarefaEntregaRepository entregas)
    {
        _currentUser = currentUser;
        _alunoCursos = alunoCursos;
        _conteudos = conteudos;
        _cursos = cursos;
        _progresso = progresso;
        _dependencias = dependencias;
        _regraAcesso = regraAcesso;
        _entregas = entregas;
    }

    public async Task<MensagemSimplesResponseDto> Handle(EntregarTarefaCommand request, CancellationToken cancellationToken)
    {
        var alunoId = _currentUser.AlunoLegadoId
            ?? throw new UnauthorizedAccessException("Aluno não identificado.");

        var conteudo = await _conteudos.ObterPorIdComDisciplinaAsync(request.ConteudoId, cancellationToken)
            ?? throw new InvalidOperationException("Conteúdo não encontrado.");

        if (conteudo.Tipo != TipoConteudo.Tarefa)
            throw new InvalidOperationException("O conteúdo não é uma tarefa.");

        if (conteudo.Disciplina is null)
            throw new InvalidOperationException("Disciplina não encontrada.");

        var curso = await _cursos.ObterPorIdComDisciplinasEConteudosAsync(conteudo.Disciplina.CursoEADId, cancellationToken)
            ?? throw new InvalidOperationException("Curso não encontrado.");

        var permitidos = await _alunoCursos.ObterIdsCursosDoAlunoAsync(alunoId, cancellationToken);
        if (!permitidos.Contains(curso.CursoLegadoId))
            throw new UnauthorizedAccessException("Curso não disponível.");

        var doc = await _progresso.ObterAsync(alunoId, curso.Id, cancellationToken);
        if (doc is null)
            doc = ProgressoCursoHelper.GarantirDocumento(alunoId, curso);
        else
            ProgressoCursoHelper.SincronizarEstrutura(doc, curso);

        var deps = await _dependencias.ListarTodasDoCursoAsync(curso.Id, cancellationToken);
        var conteudosConcluidos = ProgressoCursoHelper.ConteudosConcluidos(doc);
        var disciplinasConcluidas = ProgressoCursoHelper.DisciplinasConcluidas(curso, doc);

        if (!_regraAcesso.PodeAcessar(conteudo, deps, conteudosConcluidos, disciplinasConcluidas))
            throw new AcessoConteudoNegadoException("Conclua as dependências antes de entregar a tarefa.");

        if (string.IsNullOrWhiteSpace(request.Texto) && string.IsNullOrWhiteSpace(request.UrlArquivo))
            throw new InvalidOperationException("Informe texto ou URL do arquivo.");

        var entrega = new TarefaEntrega
        {
            Id = TarefaEntrega.MontarId(alunoId, conteudo.Id),
            AlunoLegadoId = alunoId,
            ConteudoId = conteudo.Id,
            Texto = string.IsNullOrWhiteSpace(request.Texto) ? null : request.Texto.Trim(),
            UrlArquivo = string.IsNullOrWhiteSpace(request.UrlArquivo) ? null : request.UrlArquivo.Trim(),
            EntregueEm = DateTime.UtcNow
        };

        await _entregas.SalvarAsync(entrega, cancellationToken);

        var pd = doc.Disciplinas.First(x => x.DisciplinaId == conteudo.DisciplinaEADId);
        var pc = pd.Conteudos.First(x => x.ConteudoId == conteudo.Id);
        pc.Concluido = true;
        pc.DataConclusao = DateTime.UtcNow;
        var disc = curso.Disciplinas.First(d => d.Id == conteudo.DisciplinaEADId);
        ProgressoCursoHelper.RecalcularPercentualDisciplina(pd, disc);
        await _progresso.SubstituirAsync(doc, cancellationToken);

        return new MensagemSimplesResponseDto("Tarefa entregue e conteúdo concluído.");
    }
}
