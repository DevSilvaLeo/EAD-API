namespace THEONEEAD.Domain.Tarefas;

public class TarefaEntrega
{
    public string Id { get; set; } = null!;
    public long AlunoLegadoId { get; set; }
    public Guid ConteudoId { get; set; }
    public string? Texto { get; set; }
    public string? UrlArquivo { get; set; }
    public DateTime EntregueEm { get; set; }

    public static string MontarId(long alunoLegadoId, Guid conteudoId) => $"{alunoLegadoId}:{conteudoId}";
}
