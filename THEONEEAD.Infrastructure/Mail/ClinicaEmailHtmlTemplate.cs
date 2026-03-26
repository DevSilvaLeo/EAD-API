namespace THEONEEAD.Infrastructure.Mail;

/// <summary>
/// Mesmo layout HTML usado em ClinicaControlAPI — <c>EmailService.EmailBodyGeneratorr</c>,
/// com o código vindo do fluxo EAD e data de expiração real.
/// </summary>
public static class ClinicaEmailHtmlTemplate
{
    public static string MontarCorpo(string codigo, DateTime expiraEmUtc, string? logoDataUri)
    {
        var expiraLocal = expiraEmUtc.ToLocalTime();
        var expiraTexto = expiraLocal.ToString("dd/MM/yyyy HH:mm:ss");
        var ano = DateTime.UtcNow.Year;

        var logoHtml = string.IsNullOrEmpty(logoDataUri)
            ? ""
            : $"""
                <div class="logo">
                    <img src="{logoDataUri}" alt="The One Software" />
                </div>
                """;

        // Sem interpolação no bloco principal: evita conflito com { } do CSS (mesmo HTML do Clinica).
        const string Html = """
            <!DOCTYPE html>
            <html lang="pt-br">
              <head>
                <meta charset="UTF-8">
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        background-color: #f8f9fa;
                        margin: 0;
                        padding: 20px;
                    }

                    .container {
                        max-width: 600px;
                        margin: auto;
                        background: #ffffff;
                        border-radius: 10px;
                        box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                        padding: 30px;
                    }

                    .logo {
                        text-align: left;
                        margin-bottom: 20px;
                    }

                    .logo img {
                        height: 32px;
                    }

                    h1 {
                        font-size: 24px;
                        color: #000;
                    }

                    .code-box {
                        font-size: 32px;
                        font-weight: bold;
                        color: #333;
                        background-color: #f2f2f2;
                        border-left: 6px solid #8f5cc9;
                        padding: 20px;
                        text-align: center;
                        margin: 20px 0;
                        letter-spacing: 5px;
                    }

                    ul {
                        padding-left: 20px;
                        color: #555;
                    }

                    .footer {
                        margin-top: 40px;
                        font-size: 13px;
                        text-align: center;
                        color: #999;
                    }

                    .footer strong {
                        color: #8f5cc9;
                    }

                    .help {
                        margin-top: 10px;
                        color: #555;
                    }
                </style>
              </head>
              <body>
                <div class="container">
                    __LOGO__
                    <h1>Codigo de confirmação</h1>
                    <p>Copie e cole esse código temporário de login:</p>
                    <div class="code-box">__CODIGO__</div>

                    <ul>
                        <li><strong>Importante!</strong> Não compartilhe esse código com ninguém.</li>
                        <li>Esse código tem validade limitada, expirando em __EXPIRA__.</li>
                        <li>Se você não solicitou esse código, ignore esse e-mail com segurança.</li>
                    </ul>

                    <div class="footer">
                        © __ANO__ <strong>The One Software</strong>.<br>
                        Soluções personalizadas para seu negócio!
                    </div>
                </div>
              </body>
            </html>
            """;

        return Html
            .Replace("__LOGO__", logoHtml)
            .Replace("__CODIGO__", codigo)
            .Replace("__EXPIRA__", expiraTexto)
            .Replace("__ANO__", ano.ToString(System.Globalization.CultureInfo.InvariantCulture));
    }
}
