using FluentValidation;

namespace THEONEEAD.Application.Admin.Commands;

public class CriarDependenciaCommandValidator : AbstractValidator<CriarDependenciaCommand>
{
    public CriarDependenciaCommandValidator()
    {
        RuleFor(x => x.Tipo).NotEmpty();
        RuleFor(x => x.OrigemId).NotEmpty();
        RuleFor(x => x.DestinoId).NotEmpty();
    }
}
