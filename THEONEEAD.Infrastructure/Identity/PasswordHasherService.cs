using THEONEEAD.Application.Common.Interfaces;

namespace THEONEEAD.Infrastructure.Identity;

public class PasswordHasherService : IPasswordHasher
{
    public string Hash(string senha) => BCrypt.Net.BCrypt.HashPassword(senha);

    public bool Verificar(string senha, string hash) => BCrypt.Net.BCrypt.Verify(senha, hash);
}
