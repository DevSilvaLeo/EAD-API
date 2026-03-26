using FluentValidation;

namespace THEONEEAD.Application.Auth.Commands;

public class RecuperarSenhaUnificadoCommandValidator : AbstractValidator<RecuperarSenhaUnificadoCommand>
{
    public RecuperarSenhaUnificadoCommandValidator()
    {
        RuleFor(x => x.ChaveVerificacao).NotEmpty();
        RuleFor(x => x.Codigo).NotEmpty().Length(6).Matches(@"^\d{6}$").WithMessage("O código deve ter 6 dígitos.");
        RuleFor(x => x.NovaSenha).NotEmpty();
    }
}
