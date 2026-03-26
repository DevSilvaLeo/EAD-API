using THEONEEAD.Domain.Common;

namespace THEONEEAD.Domain.Entities;

public class CursoEAD : EntityBase
{
    public long CursoLegadoId { get; private set; }
    public string Nome { get; private set; } = null!;
    public bool Sequencial { get; private set; }

    /// <summary>Coleção persistida pelo EF Core; não expor mutação fora dos métodos de domínio.</summary>
    public List<DisciplinaEAD> Disciplinas { get; private set; } = new();

    private CursoEAD() { }

    public static CursoEAD Criar(long cursoLegadoId, string nome, bool sequencial)
    {
        if (cursoLegadoId <= 0)
            throw new ArgumentException("Curso legado inválido.", nameof(cursoLegadoId));
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.", nameof(nome));

        return new CursoEAD
        {
            CursoLegadoId = cursoLegadoId,
            Nome = nome.Trim(),
            Sequencial = sequencial
        };
    }

    public void Renomear(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.", nameof(nome));
        Nome = nome.Trim();
    }

    public void DefinirSequencial(bool sequencial) => Sequencial = sequencial;

    public DisciplinaEAD AdicionarDisciplina(string nome, int ordem)
    {
        var disciplina = DisciplinaEAD.Criar(Id, nome, ordem);
        Disciplinas.Add(disciplina);
        return disciplina;
    }
}
