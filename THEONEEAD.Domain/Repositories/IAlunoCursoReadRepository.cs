namespace THEONEEAD.Domain.Repositories;

/// <summary>Vínculos aluno–curso (matrículas) no MySQL.</summary>
public interface IAlunoCursoReadRepository
{
    Task<bool> AlunoExisteAsync(long alunoId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<long>> ObterIdsCursosDoAlunoAsync(long alunoId, CancellationToken cancellationToken = default);
}
