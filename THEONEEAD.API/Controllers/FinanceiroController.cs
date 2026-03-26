using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using THEONEEAD.Application.PortalAluno;
using THEONEEAD.Application.PortalAluno.Queries;

namespace THEONEEAD.API.Controllers;

[ApiController]
[Route("api/financeiro")]
[Authorize(Policy = "SomenteAluno")]
public class FinanceiroController : ControllerBase
{
    private readonly IMediator _mediator;

    public FinanceiroController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FinanceiroItemDto>>> Listar(CancellationToken ct)
    {
        var r = await _mediator.Send(new ListarFinanceiroAlunoQuery(), ct);
        return Ok(r);
    }
}
