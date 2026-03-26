using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using THEONEEAD.Application.Auth.Commands;
using THEONEEAD.Application.Auth.Dtos;
using THEONEEAD.Application.Auth.Queries;

namespace THEONEEAD.API.Controllers;

/// <summary>
/// Autenticação pública: <c>api/auth/*</c>.
/// Dados em MySQL (base <c>seminario</c>): primeiro acesso valida o CPF em <c>estudantes</c> e cria o usuário em <c>usuarios_ead</c> se necessário.
/// Login e recuperação usam o e-mail (coluna <c>login</c>).
/// </summary>
[ApiController]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Route("api/auth/login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto body, CancellationToken ct)
    {
        var r = await _mediator.Send(new LoginCommand(body.Email, body.Senha), ct);
        return Ok(r);
    }

    [HttpPost]
    [Route("api/auth/esqueci-senha")]
    public async Task<ActionResult<EsqueciSenhaResponseDto>> EsqueciSenha([FromBody] EsqueciSenhaRequestDto body, CancellationToken ct)
    {
        var r = await _mediator.Send(new EsqueciSenhaCommand(body.Email, body.Cpf), ct);
        return Ok(r);
    }

    /// <summary>CPF consultado na tabela <c>estudantes</c>; usuário criado em <c>usuarios_ead</c> se necessário.</summary>
    [HttpPost]
    [Route("api/auth/primeiro-acesso")]
    public async Task<ActionResult<PrimeiroAcessoResponseDto>> PrimeiroAcesso([FromBody] PrimeiroAcessoRequestDto body, CancellationToken ct)
    {
        var r = await _mediator.Send(new PrimeiroAcessoSolicitarCommand(body.Cpf), ct);
        return Ok(r);
    }

    [HttpPost]
    [Route("api/auth/validar-codigo")]
    public async Task<ActionResult<ValidarCodigoResponseDto>> ValidarCodigo([FromBody] ValidarCodigoRequestDto body, CancellationToken ct)
    {
        var r = await _mediator.Send(new ValidarCodigoCommand(body.Codigo, body.ChaveVerificacao), ct);
        return Ok(r);
    }

    [HttpPost]
    [Route("api/auth/trocar-senha")]
    public async Task<ActionResult<TrocarSenhaResponseDto>> TrocarSenha([FromBody] TrocarSenhaRequestDto body, CancellationToken ct)
    {
        var r = await _mediator.Send(
            new TrocarSenhaFrontendCommand(body.Senha, body.SenhaConfirmacao, body.Token),
            ct);
        return Ok(r);
    }

    [HttpPost]
    [Route("api/auth/recuperar-senha")]
    public async Task<ActionResult<TrocarSenhaResponseDto>> RecuperarSenha([FromBody] RecuperarSenhaUnificadoRequestDto body, CancellationToken ct)
    {
        var r = await _mediator.Send(
            new RecuperarSenhaUnificadoCommand(
                body.RecuperaSenhaToken ?? "",
                body.Code ?? "",
                body.NovaSenha ?? "",
                body.NovaSenha),
            ct);

        if (!r.Mensagem.StartsWith("Senha alterada", StringComparison.OrdinalIgnoreCase))
            return BadRequest(r);

        return Ok(r);
    }

    [HttpPost]
    [Route("api/auth/reseta-primeiro-acesso")]
    public async Task<ActionResult<ResetaPrimeiroAcessoResponseDto>> ResetaPrimeiroAcesso(
        [FromBody] ResetaPrimeiroAcessoRequestDto body,
        CancellationToken ct)
    {
        var chave = body.ChaveVerificacao?.Trim() ?? body.TokenAcesso?.Trim();
        if (string.IsNullOrEmpty(chave))
        {
            return Ok(new ResetaPrimeiroAcessoResponseDto(201, "Token de acesso ausente.", null));
        }

        var chaveAtiva = await _mediator.Send(new ConsultarChaveVerificacaoQuery(chave), ct);
        if (chaveAtiva)
        {
            return Ok(new ResetaPrimeiroAcessoResponseDto(201, "O código anterior ainda está válido.", null));
        }

        return Ok(new ResetaPrimeiroAcessoResponseDto(
            201,
            "Solicite novamente o primeiro acesso informando o CPF.",
            null));
    }
}
