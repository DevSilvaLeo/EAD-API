using FluentValidation;

namespace THEONEEAD.Application.Cursos.Commands;

public class ConcluirConteudoCommandValidator : AbstractValidator<ConcluirConteudoCommand>
{
    public ConcluirConteudoCommandValidator()
    {
        RuleFor(x => x.CursoId).NotEmpty();
        RuleFor(x => x.ConteudoId).NotEmpty();
    }
}
