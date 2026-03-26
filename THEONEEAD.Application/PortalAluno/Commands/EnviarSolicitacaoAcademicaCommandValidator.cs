using FluentValidation;

namespace THEONEEAD.Application.PortalAluno.Commands;

public class EnviarSolicitacaoAcademicaCommandValidator : AbstractValidator<EnviarSolicitacaoAcademicaCommand>
{
    public EnviarSolicitacaoAcademicaCommandValidator()
    {
        RuleFor(x => x.Request).NotNull();
        When(x => x.Request != null, () =>
        {
            RuleFor(x => x.Request!.TipoSolicitacaoId).NotEmpty();
            RuleFor(x => x.Request!.Descricao).NotEmpty().MaximumLength(4000);
        });
    }
}
