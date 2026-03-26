namespace THEONEEAD.Domain.Progresso;

public class ProgressoDisciplinaAluno
{
    public Guid DisciplinaId { get; set; }
    public decimal Percentual { get; set; }
    public List<ProgressoConteudoAluno> Conteudos { get; set; } = new();
}
