using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using THEONEEAD.Application.PortalAluno;
using THEONEEAD.Application.PortalAluno.Commands;
using THEONEEAD.Application.PortalAluno.Queries;

namespace THEONEEAD.API.Controllers;

[ApiController]
[Route("api/solicitacoes-academicas")]
[Authorize(Policy = "SomenteAluno")]
public class SolicitacoesAcademicasController : ControllerBase
{
    private readonly IMediator _mediator;

    public SolicitacoesAcademicasController(IMediator mediator) => _mediator = mediator;

    [HttpGet("meus-dados")]
    public async Task<ActionResult<DadosAlunoSolicitacaoDto>> MeusDados(CancellationToken ct)
    {
        var r = await _mediator.Send(new ObterMeusDadosSolicitacaoQuery(), ct);
        if (r is null)
            return NotFound();
        return Ok(r);
    }

    [HttpGet("tipos")]
    public async Task<ActionResult<IReadOnlyList<TipoSolicitacaoDto>>> Tipos(CancellationToken ct)
    {
        var r = await _mediator.Send(new ListarTiposSolicitacaoQuery(), ct);
        return Ok(r);
    }

    [HttpPost]
    public async Task<ActionResult<SolicitacaoAcademicaResponseDto>> Enviar([FromBody] SolicitacaoAcademicaRequestDto body, CancellationToken ct)
    {
        var r = await _mediator.Send(new EnviarSolicitacaoAcademicaCommand(body), ct);
        return Ok(r);
    }
}
