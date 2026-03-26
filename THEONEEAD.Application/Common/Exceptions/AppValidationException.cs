namespace THEONEEAD.Application.Common.Exceptions;

public class AppValidationException : Exception
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public AppValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base("Erro de validação.")
    {
        Errors = errors;
    }
}
