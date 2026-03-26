using FluentValidation;

namespace THEONEEAD.Application.Auth.Commands;

public class TrocarSenhaFrontendCommandValidator : AbstractValidator<TrocarSenhaFrontendCommand>
{
    public TrocarSenhaFrontendCommandValidator()
    {
        RuleFor(x => x.Senha).NotEmpty();
        RuleFor(x => x.SenhaConfirmacao).NotEmpty();
        RuleFor(x => x.Token).NotEmpty();
    }
}
