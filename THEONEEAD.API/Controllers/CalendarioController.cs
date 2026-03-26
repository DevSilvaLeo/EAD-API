using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using THEONEEAD.Application.PortalAluno;
using THEONEEAD.Application.PortalAluno.Queries;

namespace THEONEEAD.API.Controllers;

[ApiController]
[Route("api/calendario")]
[Authorize(Policy = "SomenteAluno")]
public class CalendarioController : ControllerBase
{
    private readonly IMediator _mediator;

    public CalendarioController(IMediator mediator) => _mediator = mediator;

    [HttpGet("eventos")]
    public async Task<ActionResult<IReadOnlyList<EventoCalendarioDto>>> Eventos(CancellationToken ct)
    {
        var r = await _mediator.Send(new ListarEventosCalendarioQuery(), ct);
        return Ok(r);
    }
}
