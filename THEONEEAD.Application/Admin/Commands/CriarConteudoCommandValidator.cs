using FluentValidation;

namespace THEONEEAD.Application.Admin.Commands;

public class CriarConteudoCommandValidator : AbstractValidator<CriarConteudoCommand>
{
    public CriarConteudoCommandValidator()
    {
        RuleFor(x => x.DisciplinaEADId).NotEmpty();
        RuleFor(x => x.Titulo).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Tipo).NotEmpty();
        RuleFor(x => x.Ordem).GreaterThanOrEqualTo(0);
    }
}
