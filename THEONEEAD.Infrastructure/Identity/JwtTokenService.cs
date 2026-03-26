using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using THEONEEAD.Application.Auth;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Domain.Entities;
using THEONEEAD.Domain.Enums;

namespace THEONEEAD.Infrastructure.Identity;

public class JwtTokenService : IJwtTokenService
{
    public const string ClaimPurpose = "purpose";
    public const string PurposePasswordReset = "password_reset";

    private readonly JwtOptions _opt;

    public JwtTokenService(IOptions<JwtOptions> opt) => _opt = opt.Value;

    public string GerarAccessToken(Usuario usuario, TimeSpan validade)
    {
        var role = usuario.Perfil == PerfilUsuario.Administrador ? "Administrador" : "Aluno";
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString(CultureInfo.InvariantCulture)),
            new(JwtRegisteredClaimNames.Email, usuario.Email),
            new(ClaimTypes.Name, usuario.Nome),
            new(ClaimTypes.Role, role)
        };

        if (usuario.AlunoLegadoId is long aluno)
            claims.Add(new Claim("aluno_legado_id", aluno.ToString(CultureInfo.InvariantCulture)));

        return CriarJwt(claims, validade);
    }

    public string GerarTokenRedefinicaoSenha(int usuarioId, TimeSpan validade)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuarioId.ToString(CultureInfo.InvariantCulture)),
            new(ClaimPurpose, PurposePasswordReset)
        };
        return CriarJwt(claims, validade);
    }

    public int? ObterUsuarioIdDoTokenRedefinicao(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return null;

        var handler = new JwtSecurityTokenHandler { MapInboundClaims = false };
        try
        {
            var principal = handler.ValidateToken(token, CriarValidationParameters(), out _);
            if (principal.FindFirst(ClaimPurpose)?.Value != PurposePasswordReset)
                return null;
            var sub = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            return int.TryParse(sub, NumberStyles.Integer, CultureInfo.InvariantCulture, out var id) ? id : null;
        }
        catch
        {
            return null;
        }
    }

    public PerfilUsuario? ObterPerfilDaClaim(string? claimValue) =>
        claimValue switch
        {
            "Administrador" => PerfilUsuario.Administrador,
            "Aluno" => PerfilUsuario.Aluno,
            _ => null
        };

    private string CriarJwt(IEnumerable<Claim> claims, TimeSpan validade)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwt = new JwtSecurityToken(
            issuer: _opt.Issuer,
            audience: _opt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(validade),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private TokenValidationParameters CriarValidationParameters() =>
        new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _opt.Issuer,
            ValidAudience = _opt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.SigningKey)),
            ClockSkew = TimeSpan.FromMinutes(1),
            NameClaimType = JwtRegisteredClaimNames.Sub
        };
}
