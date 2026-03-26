using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using THEONEEAD.Application.Auth.Dtos;
using THEONEEAD.Application.Cursos.Commands;
using THEONEEAD.Application.Cursos.Dtos;
using THEONEEAD.Application.Cursos.Queries;

namespace THEONEEAD.API.Controllers;

[ApiController]
[Route("api/cursos")]
[Authorize(Policy = "SomenteAluno")]
public class CursosController : ControllerBase
{
    private readonly IMediator _mediator;

    public CursosController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CursoListaFrontendItemDto>>> Listar(CancellationToken ct)
    {
        var r = await _mediator.Send(new ListarCursosAlunoQuery(), ct);
        return Ok(r);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CursoDetalheFrontendDto>> Obter(string id, CancellationToken ct)
    {
        if (!Guid.TryParse(id, out var cursoId))
            return NotFound();
        var r = await _mediator.Send(new ObterCursoDetalheQuery(cursoId), ct);
        if (r is null)
            return NotFound();
        return Ok(r);
    }

    /// <summary>Rota legada interna (conteúdo).</summary>
    [HttpPost("{cursoId}/conteudos/{conteudoId}/concluir")]
    public async Task<ActionResult<MensagemSimplesResponseDto>> ConcluirConteudo(Guid cursoId, Guid conteudoId, CancellationToken ct)
    {
        var r = await _mediator.Send(new ConcluirConteudoCommand(cursoId, conteudoId), ct);
        return Ok(r);
    }

    /// <summary>Contrato Angular: aula = conteúdo.</summary>
    [HttpPost("{cursoId}/aulas/{aulaId}/concluir")]
    public async Task<ActionResult<MensagemSimplesResponseDto>> ConcluirAula(string cursoId, string aulaId, CancellationToken ct)
    {
        if (!Guid.TryParse(cursoId, out var cId) || !Guid.TryParse(aulaId, out var aId))
            return BadRequest();
        var r = await _mediator.Send(new ConcluirConteudoCommand(cId, aId), ct);
        return Ok(r);
    }
}
