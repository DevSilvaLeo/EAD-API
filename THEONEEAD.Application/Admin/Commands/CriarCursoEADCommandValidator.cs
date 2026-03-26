using FluentValidation;

namespace THEONEEAD.Application.Admin.Commands;

public class CriarCursoEADCommandValidator : AbstractValidator<CriarCursoEADCommand>
{
    public CriarCursoEADCommandValidator()
    {
        RuleFor(x => x.CursoLegadoId).GreaterThan(0);
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(500);
    }
}
