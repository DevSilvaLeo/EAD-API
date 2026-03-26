namespace THEONEEAD.Domain.Progresso;

public class ProgressoConteudoAluno
{
    public Guid ConteudoId { get; set; }
    public bool Concluido { get; set; }
    public DateTime? DataConclusao { get; set; }
}
