using FluentValidation;

namespace THEONEEAD.Application.Auth.Commands;

public class ValidarCodigoCommandValidator : AbstractValidator<ValidarCodigoCommand>
{
    public ValidarCodigoCommandValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().Length(6).Matches(@"^\d{6}$").WithMessage("O código deve ter 6 dígitos.");
        RuleFor(x => x.ChaveVerificacao).NotEmpty().WithMessage("Chave de verificação é obrigatória.");
    }
}
