using FluentValidation;

namespace THEONEEAD.Application.Auth.Commands;

public class PrimeiroAcessoSolicitarCommandValidator : AbstractValidator<PrimeiroAcessoSolicitarCommand>
{
    public PrimeiroAcessoSolicitarCommandValidator()
    {
        RuleFor(x => x.Cpf).NotEmpty();
    }
}
