using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using THEONEEAD.Application.Auth.Dtos;
using THEONEEAD.Application.Tarefas.Commands;
using THEONEEAD.Application.Tarefas.Dtos;
using THEONEEAD.Application.Tarefas.Queries;

namespace THEONEEAD.API.Controllers;

[ApiController]
[Route("api/tarefas")]
[Authorize(Policy = "SomenteAluno")]
public class TarefasController : ControllerBase
{
    private readonly IMediator _mediator;

    public TarefasController(IMediator mediator) => _mediator = mediator;

    [HttpPost("{conteudoId:guid}/entregar")]
    public async Task<ActionResult<MensagemSimplesResponseDto>> Entregar(Guid conteudoId, [FromBody] EntregarTarefaRequestDto body, CancellationToken ct)
    {
        var r = await _mediator.Send(new EntregarTarefaCommand(conteudoId, body.Texto, body.UrlArquivo), ct);
        return Ok(r);
    }

    [HttpGet("pendentes")]
    public async Task<ActionResult<IReadOnlyList<TarefaPendenteFrontendDto>>> Pendentes(CancellationToken ct)
    {
        var r = await _mediator.Send(new ListarTarefasPendentesQuery(), ct);
        return Ok(r);
    }
}
