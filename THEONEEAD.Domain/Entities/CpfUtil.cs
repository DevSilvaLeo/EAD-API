namespace THEONEEAD.Domain.Entities;

public static class CpfUtil
{
    public static string? Normalizar(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return null;
        var digits = new string(cpf.Where(char.IsDigit).ToArray());
        return digits.Length == 0 ? null : digits;
    }
}
