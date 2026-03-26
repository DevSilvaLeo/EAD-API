using FluentValidation;

namespace THEONEEAD.Application.Admin.Commands;

public class CriarDisciplinaEADCommandValidator : AbstractValidator<CriarDisciplinaEADCommand>
{
    public CriarDisciplinaEADCommandValidator()
    {
        RuleFor(x => x.CursoEADId).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Ordem).GreaterThanOrEqualTo(0);
    }
}
