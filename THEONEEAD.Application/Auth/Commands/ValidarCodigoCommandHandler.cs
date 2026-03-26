using MediatR;
using Microsoft.Extensions.Options;
using THEONEEAD.Application.Auth.Dtos;
using THEONEEAD.Application.Common.Interfaces;

namespace THEONEEAD.Application.Auth.Commands;

public class ValidarCodigoCommandHandler : IRequestHandler<ValidarCodigoCommand, ValidarCodigoResponseDto>
{
    private readonly ICodigoVerificacaoStore _store;
    private readonly IJwtTokenService _jwt;
    private readonly JwtOptions _jwtOptions;

    public ValidarCodigoCommandHandler(
        ICodigoVerificacaoStore store,
        IJwtTokenService jwt,
        IOptions<JwtOptions> jwtOptions)
    {
        _store = store;
        _jwt = jwt;
        _jwtOptions = jwtOptions.Value;
    }

    public Task<ValidarCodigoResponseDto> Handle(ValidarCodigoCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChaveVerificacao))
            return Task.FromResult(new ValidarCodigoResponseDto(
                string.Empty,
                "Chave de verificação ausente. Solicite novamente o código."));

        if (!_store.TentarValidarEConsumir(
                request.ChaveVerificacao.Trim(),
                request.Codigo.Trim(),
                out var usuarioId,
                out _))
            return Task.FromResult(new ValidarCodigoResponseDto(
                string.Empty,
                "Código inválido ou expirado. Verifique e tente novamente."));

        var minutos = _jwtOptions.RedefinicaoSenhaMinutos > 0 ? _jwtOptions.RedefinicaoSenhaMinutos : 15;
        var token = _jwt.GerarTokenRedefinicaoSenha(usuarioId, TimeSpan.FromMinutes(minutos));
        return Task.FromResult(new ValidarCodigoResponseDto(
            token,
            "Código validado com sucesso. Defina sua nova senha."));
    }
}
