using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using THEONEEAD.Application.Auth.Dtos;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Application.Forum.Commands;
using THEONEEAD.Application.Forum.Dtos;
using THEONEEAD.Application.Forum.Queries;

namespace THEONEEAD.API.Controllers;

[ApiController]
[Route("api/forum")]
[Authorize]
public class ForumController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public ForumController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet("{tipo}/{id}")]
    public async Task<ActionResult<ForumThreadResponseDto>> Obter(string tipo, string id, CancellationToken ct)
    {
        var r = await _mediator.Send(new ObterForumQuery(tipo, id), ct);
        return Ok(r);
    }

    [HttpPost("{tipo}/{id}")]
    public async Task<ActionResult<MensagemSimplesResponseDto>> Publicar(string tipo, string id, [FromBody] ForumPostRequestDto body, CancellationToken ct)
    {
        var autorId = _currentUser.AlunoLegadoId?.ToString()
            ?? _currentUser.UserId?.ToString(System.Globalization.CultureInfo.InvariantCulture)
            ?? throw new UnauthorizedAccessException("Usuário não identificado.");

        var r = await _mediator.Send(new PublicarForumCommand(tipo, id, body.Mensagem, body.ParentEntradaId, autorId), ct);
        return Ok(r);
    }
}
