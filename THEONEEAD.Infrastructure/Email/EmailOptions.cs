namespace THEONEEAD.Infrastructure.Email;

public class EmailOptions
{
    public const string SectionName = "Email";
    public string From { get; set; } = "";
    public string Password { get; set; } = "";
    public string Smtp { get; set; } = "";
    public int Port { get; set; } = 587;
    /// <summary>Nome exibido no remetente (igual Clinica: "Suporte").</summary>
    public string DisplayName { get; set; } = "Suporte";
    /// <summary>Caminho relativo à raiz da API (ContentRoot), ex.: wwwroot/Assets/Images/logo_ONE-ret.png</summary>
    public string? LogoPath { get; set; }
}
