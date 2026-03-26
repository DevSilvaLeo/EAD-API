using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using THEONEEAD.Application.Admin.Commands;
using THEONEEAD.Application.Admin.Dtos;

namespace THEONEEAD.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Policy = "SomenteAdmin")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator) => _mediator = mediator;

    [HttpPost("cursos-eada")]
    public async Task<ActionResult<RecursoCriadoResponseDto>> CriarCurso([FromBody] CriarCursoEadRequestDto body, CancellationToken ct)
    {
        var r = await _mediator.Send(new CriarCursoEADCommand(body.CursoLegadoId, body.Nome, body.Sequencial), ct);
        return Ok(r);
    }

    [HttpPost("disciplinas")]
    public async Task<ActionResult<RecursoCriadoResponseDto>> CriarDisciplina([FromBody] CriarDisciplinaRequestDto body, CancellationToken ct)
    {
        var r = await _mediator.Send(new CriarDisciplinaEADCommand(body.CursoEADId, body.Nome, body.Ordem), ct);
        return Ok(r);
    }

    [HttpPost("conteudos")]
    public async Task<ActionResult<RecursoCriadoResponseDto>> CriarConteudo([FromBody] CriarConteudoRequestDto body, CancellationToken ct)
    {
        var r = await _mediator.Send(
            new CriarConteudoCommand(
                body.DisciplinaEADId,
                body.Titulo,
                body.Tipo,
                body.Ordem,
                body.UrlVideo,
                body.UrlSlides,
                body.ConteudoTexto,
                body.DuracaoSegundos),
            ct);
        return Ok(r);
    }

    [HttpPost("dependencias")]
    public async Task<ActionResult<RecursoCriadoResponseDto>> CriarDependencia([FromBody] CriarDependenciaRequestDto body, CancellationToken ct)
    {
        var r = await _mediator.Send(new CriarDependenciaCommand(body.Tipo, body.OrigemId, body.DestinoId), ct);
        return Ok(r);
    }
}
