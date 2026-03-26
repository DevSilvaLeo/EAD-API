using THEONEEAD.Domain.Enums;

namespace THEONEEAD.Application.Common.Interfaces;

public interface ICurrentUserService
{
    int? UserId { get; }

    long? AlunoLegadoId { get; }

    PerfilUsuario? Perfil { get; }

    bool IsAuthenticated { get; }

    string? Email { get; }

    string? Nome { get; }
}
