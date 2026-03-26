using FluentValidation;

namespace THEONEEAD.Application.Auth.Commands;

public class EsqueciSenhaCommandValidator : AbstractValidator<EsqueciSenhaCommand>
{
    public EsqueciSenhaCommandValidator()
    {
        RuleFor(x => x)
            .Must(c => !string.IsNullOrWhiteSpace(c.Email) || !string.IsNullOrWhiteSpace(c.Cpf))
            .WithMessage("Informe o e-mail ou o CPF cadastrado.");
    }
}
