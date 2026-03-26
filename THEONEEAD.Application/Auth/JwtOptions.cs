namespace THEONEEAD.Application.Auth;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    public string Issuer { get; set; } = "";
    public string Audience { get; set; } = "";
    public string SigningKey { get; set; } = "";
    public int ExpiracaoMinutos { get; set; } = 60;
    /// <summary>Validade do JWT de redefinição de senha (após validar código).</summary>
    public int RedefinicaoSenhaMinutos { get; set; } = 15;
}
