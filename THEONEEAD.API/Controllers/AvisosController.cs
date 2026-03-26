using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using THEONEEAD.Application.PortalAluno;
using THEONEEAD.Application.PortalAluno.Queries;

namespace THEONEEAD.API.Controllers;

[ApiController]
[Route("api/avisos")]
[Authorize(Policy = "SomenteAluno")]
public class AvisosController : ControllerBase
{
    private readonly IMediator _mediator;

    public AvisosController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AvisoDto>>> Listar(CancellationToken ct)
    {
        var r = await _mediator.Send(new ListarAvisosAlunoQuery(), ct);
        return Ok(r);
    }
}
