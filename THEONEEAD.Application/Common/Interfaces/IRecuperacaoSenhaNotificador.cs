namespace THEONEEAD.Application.Common.Interfaces;

/// <summary>
/// Envio de e-mail com código — mesmo padrão Clinica (<c>Assunto</c> + corpo HTML).
/// </summary>
public interface IRecuperacaoSenhaNotificador
{
    /// <param name="assunto">Ex.: "Confirme seu acesso." ou "Confirme seu email de recuperação."</param>
    /// <param name="expiraEmUtc">Data/hora UTC em que o código deixa de valer (exibida no corpo do e-mail).</param>
    Task NotificarCodigoAsync(
        string email,
        string codigo,
        string assunto,
        DateTime expiraEmUtc,
        CancellationToken cancellationToken = default);
}
