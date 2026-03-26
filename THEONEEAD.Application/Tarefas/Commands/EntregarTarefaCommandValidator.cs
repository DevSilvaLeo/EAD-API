using FluentValidation;

namespace THEONEEAD.Application.Tarefas.Commands;

public class EntregarTarefaCommandValidator : AbstractValidator<EntregarTarefaCommand>
{
    public EntregarTarefaCommandValidator()
    {
        RuleFor(x => x.ConteudoId).NotEmpty();
        RuleFor(x => x)
            .Must(c => !string.IsNullOrWhiteSpace(c.Texto) || !string.IsNullOrWhiteSpace(c.UrlArquivo))
            .WithMessage("Informe texto ou URL do arquivo.");
    }
}
