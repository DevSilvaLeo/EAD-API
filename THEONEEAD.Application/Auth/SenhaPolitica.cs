using System.Text.RegularExpressions;

namespace THEONEEAD.Application.Auth;

public static class SenhaPolitica
{
    private static readonly Regex Padrao = new(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%]).{8,13}$",
        RegexOptions.Compiled);

    public static bool Validar(string senha, out string mensagemErro)
    {
        mensagemErro = "";
        if (string.IsNullOrWhiteSpace(senha))
        {
            mensagemErro = "Informe a senha.";
            return false;
        }

        if (!Padrao.IsMatch(senha))
        {
            mensagemErro = "A senha deve ter 8 a 13 caracteres, com maiúscula, minúscula, número e um dos caracteres !@#$.";
            return false;
        }

        return true;
    }
}
