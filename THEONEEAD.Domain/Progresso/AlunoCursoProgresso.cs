namespace THEONEEAD.Domain.Progresso;

public class AlunoCursoProgresso
{
    public string Id { get; set; } = null!;
    public long AlunoId { get; set; }
    public Guid CursoId { get; set; }
    public List<ProgressoDisciplinaAluno> Disciplinas { get; set; } = new();

    public static string MontarId(long alunoLegadoId, Guid cursoEADId) => $"{alunoLegadoId}:{cursoEADId}";
}
