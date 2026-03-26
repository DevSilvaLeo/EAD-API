namespace THEONEEAD.Infrastructure.Persistence.Seminario.Entities;

/// <summary>Vínculo aluno–curso (matrícula).</summary>
public class AlunoCurso
{
    public long AlunoId { get; set; }
    public long CursoId { get; set; }
}
