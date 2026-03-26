namespace THEONEEAD.Domain.Forum;

public class ForumEntrada
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string AlunoId { get; set; } = null!;
    public string Mensagem { get; set; } = null!;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public List<ForumResposta> Respostas { get; set; } = new();
}
