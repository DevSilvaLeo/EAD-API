using THEONEEAD.Domain.Entities;
using THEONEEAD.Domain.Enums;

namespace THEONEEAD.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GerarAccessToken(Usuario usuario, TimeSpan validade);

    string GerarTokenRedefinicaoSenha(int usuarioId, TimeSpan validade);

    int? ObterUsuarioIdDoTokenRedefinicao(string token);

    PerfilUsuario? ObterPerfilDaClaim(string? claimValue);
}
