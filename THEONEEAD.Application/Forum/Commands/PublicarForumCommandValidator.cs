using FluentValidation;

namespace THEONEEAD.Application.Forum.Commands;

public class PublicarForumCommandValidator : AbstractValidator<PublicarForumCommand>
{
    public PublicarForumCommandValidator()
    {
        RuleFor(x => x.TipoRota).NotEmpty();
        RuleFor(x => x.ReferenciaId).NotEmpty();
        RuleFor(x => x.Mensagem).NotEmpty().MaximumLength(8000);
        RuleFor(x => x.AlunoIdAutor).NotEmpty();
    }
}
