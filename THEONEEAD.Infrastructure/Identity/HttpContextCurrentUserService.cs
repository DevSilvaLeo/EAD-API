using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Domain.Enums;

namespace THEONEEAD.Infrastructure.Identity;

public class HttpContextCurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _http;

    public HttpContextCurrentUserService(IHttpContextAccessor http) => _http = http;

    private ClaimsPrincipal? User => _http.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public int? UserId
    {
        get
        {
            var v = User?.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return int.TryParse(v, NumberStyles.Integer, CultureInfo.InvariantCulture, out var id) ? id : null;
        }
    }

    public long? AlunoLegadoId =>
        long.TryParse(User?.FindFirst("aluno_legado_id")?.Value, out var a) ? a : null;

    public PerfilUsuario? Perfil =>
        User?.FindFirst(ClaimTypes.Role)?.Value switch
        {
            "Administrador" => PerfilUsuario.Administrador,
            "Aluno" => PerfilUsuario.Aluno,
            _ => null
        };

    public string? Email =>
        User?.FindFirstValue(ClaimTypes.Email)
        ?? User?.FindFirstValue(JwtRegisteredClaimNames.Email);

    public string? Nome => User?.FindFirstValue(ClaimTypes.Name);
}
