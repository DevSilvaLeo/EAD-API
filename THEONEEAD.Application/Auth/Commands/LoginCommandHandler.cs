using MediatR;
using Microsoft.Extensions.Options;
using THEONEEAD.Application.Auth.Dtos;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Domain.Enums;
using THEONEEAD.Domain.Repositories;

namespace THEONEEAD.Application.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IEstudanteReadRepository _estudantes;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwt;
    private readonly JwtOptions _jwtOptions;

    public LoginCommandHandler(
        IUsuarioRepository usuarios,
        IEstudanteReadRepository estudantes,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwt,
        IOptions<JwtOptions> jwtOptions)
    {
        _usuarios = usuarios;
        _estudantes = estudantes;
        _passwordHasher = passwordHasher;
        _jwt = jwt;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var usuario = await _usuarios.ObterPorEmailAsync(email, cancellationToken);

        if (usuario is null)
            return new LoginResponseDto(null, null, "Email ou senha inválidos. Verifique suas credenciais.");

        if (usuario.PrimeiroAcessoPendente)
            return new LoginResponseDto(null, null, "Conclua o primeiro acesso antes de entrar.");

        if (!_passwordHasher.Verificar(request.Senha, usuario.SenhaHash))
            return new LoginResponseDto(null, null, "Email ou senha inválidos. Verifique suas credenciais.");

        if (usuario.Perfil == PerfilUsuario.Administrador)
            usuario.DefinirContextoJwt("Administrador", null);
        else
        {
            var aluno = await _estudantes.ObterPorEmailNormalizadoAsync(email, cancellationToken);
            if (aluno is null || string.IsNullOrEmpty(aluno.EmailNormalizado))
                return new LoginResponseDto(null, null, "Email ou senha inválidos. Verifique suas credenciais.");

            usuario.DefinirContextoJwt(aluno.Nome, aluno.Id);
        }

        var token = _jwt.GerarAccessToken(usuario, TimeSpan.FromMinutes(_jwtOptions.ExpiracaoMinutos));
        var agora = DateTime.UtcNow.ToString("O");
        return new LoginResponseDto(token, agora, "Acesso concedido");
    }
}
