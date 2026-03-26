using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Infrastructure.Email;
using THEONEEAD.Infrastructure.Mail;

namespace THEONEEAD.Infrastructure.Identity;

/// <summary>
/// Equivalente ao envio de <c>EnviarCodigoConfirmacao</c> do ClinicaControl (SMTP + modelo HTML).
/// </summary>
public class RecuperacaoSenhaNotificador : IRecuperacaoSenhaNotificador
{
    private readonly ILogger<RecuperacaoSenhaNotificador> _logger;
    private readonly EmailOptions _email;
    private readonly IHostEnvironment _env;

    public RecuperacaoSenhaNotificador(
        ILogger<RecuperacaoSenhaNotificador> logger,
        IOptions<EmailOptions> emailOptions,
        IHostEnvironment env)
    {
        _logger = logger;
        _email = emailOptions.Value;
        _env = env;
    }

    public async Task NotificarCodigoAsync(
        string emailDestino,
        string codigo,
        string assunto,
        DateTime expiraEmUtc,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_email.Smtp) || string.IsNullOrWhiteSpace(_email.From))
        {
            _logger.LogWarning(
                "E-mail SMTP não configurado (Email:Smtp / Email:From). Código para {Email}: {Codigo}",
                emailDestino, codigo);
            return;
        }

        var logoDataUri = CarregarLogoComoDataUri();

        var html = ClinicaEmailHtmlTemplate.MontarCorpo(codigo, expiraEmUtc, logoDataUri);

        var remetente = new MailAddress(_email.From.Trim(), _email.DisplayName.Trim());
        var destinatario = new MailAddress(emailDestino.Trim());

        using var msg = new MailMessage(remetente, destinatario)
        {
            Subject = assunto,
            Body = html,
            IsBodyHtml = true
        };

        using var client = new SmtpClient(_email.Smtp, _email.Port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(_email.From.Trim(), _email.Password)
        };

        try
        {
            await client.SendMailAsync(msg, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao enviar e-mail de verificação para {Email}", emailDestino);
            throw;
        }
    }

    private string? CarregarLogoComoDataUri()
    {
        var rel = _email.LogoPath?.Trim();
        if (string.IsNullOrEmpty(rel))
            return null;

        var full = Path.Combine(_env.ContentRootPath, rel.Replace('/', Path.DirectorySeparatorChar));
        if (!File.Exists(full))
        {
            _logger.LogWarning("Logo de e-mail não encontrada em {Path}. Envie sem imagem.", full);
            return null;
        }

        try
        {
            var bytes = File.ReadAllBytes(full);
            var b64 = Convert.ToBase64String(bytes);
            var ext = Path.GetExtension(full).ToLowerInvariant();
            var mime = ext switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "image/png"
            };
            return $"data:{mime};base64,{b64}";
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Não foi possível ler o logo de e-mail.");
            return null;
        }
    }
}
