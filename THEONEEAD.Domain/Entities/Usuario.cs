using System.ComponentModel.DataAnnotations.Schema;
using THEONEEAD.Domain.Enums;

namespace THEONEEAD.Domain.Entities;

/// <summary>Usuário EAD persistido em <c>usuarios_ead</c> (MySQL, base <c>seminario</c>).</summary>
public class Usuario
{
    public int Id { get; private set; }

    public bool Status { get; private set; }

    /// <summary>E-mail de login (coluna <c>login</c>).</summary>
    public string Login { get; private set; } = null!;

    /// <summary>Hash da senha (coluna <c>senha</c>).</summary>
    public string SenhaHash { get; private set; } = null!;

    public PerfilUsuario Perfil { get; private set; }

    /// <summary>Uso futuro ou integração legada (coluna <c>recupera_senha</c>).</summary>
    public string? RecuperaSenha { get; private set; }

    /// <summary>Quando igual a <c>1</c>, o aluno ainda não definiu senha definitiva (primeiro acesso).</summary>
    public string? PrimeiroAcesso { get; private set; }

    [NotMapped]
    public string Nome { get; private set; } = "";

    [NotMapped]
    public long? AlunoLegadoId { get; private set; }

    public string Email => Login;

    public bool PrimeiroAcessoPendente =>
        Perfil == PerfilUsuario.Aluno && PrimeiroAcesso == MarcadorPrimeiroAcessoPendente;

    private const string MarcadorPrimeiroAcessoPendente = "1";

    private Usuario() { }

    public static Usuario CriarRegistroAlunoPrimeiroAcesso(string login, string senhaHash)
    {
        ValidarLogin(login);
        if (string.IsNullOrWhiteSpace(senhaHash))
            throw new ArgumentException("Hash de senha inválido.", nameof(senhaHash));

        return new Usuario
        {
            Status = true,
            Login = login.Trim().ToLowerInvariant(),
            SenhaHash = senhaHash,
            Perfil = PerfilUsuario.Aluno,
            PrimeiroAcesso = MarcadorPrimeiroAcessoPendente
        };
    }

    public static Usuario CriarAdministrador(string email, string nome, string senhaHash)
    {
        ValidarLogin(email);
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.", nameof(nome));
        if (string.IsNullOrWhiteSpace(senhaHash))
            throw new ArgumentException("Hash de senha inválido.", nameof(senhaHash));

        return new Usuario
        {
            Status = true,
            Login = email.Trim().ToLowerInvariant(),
            SenhaHash = senhaHash,
            Perfil = PerfilUsuario.Administrador,
            PrimeiroAcesso = null
        };
    }

    public void DefinirContextoJwt(string nome, long? alunoLegadoId)
    {
        Nome = nome.Trim();
        AlunoLegadoId = alunoLegadoId;
    }

    public void ConcluirPrimeiroAcesso(string novaSenhaHash)
    {
        if (string.IsNullOrWhiteSpace(novaSenhaHash))
            throw new ArgumentException("Senha inválida.", nameof(novaSenhaHash));
        SenhaHash = novaSenhaHash;
        PrimeiroAcesso = null;
    }

    public void TrocarSenhaComCodigo(string novaSenhaHash)
    {
        if (string.IsNullOrWhiteSpace(novaSenhaHash))
            throw new ArgumentException("Senha inválida.", nameof(novaSenhaHash));
        SenhaHash = novaSenhaHash;
        RecuperaSenha = null;
    }

    public void DefinirNovaSenhaAutenticado(string novaSenhaHash)
    {
        if (string.IsNullOrWhiteSpace(novaSenhaHash))
            throw new ArgumentException("Senha inválida.", nameof(novaSenhaHash));
        SenhaHash = novaSenhaHash;
    }

    private static void ValidarLogin(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
            throw new ArgumentException("Login (e-mail) é obrigatório.", nameof(login));
    }
}
