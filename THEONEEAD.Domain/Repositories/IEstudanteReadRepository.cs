using THEONEEAD.Domain.Estudantes;

namespace THEONEEAD.Domain.Repositories;

/// <summary>Leitura da tabela <c>estudantes</c> (CPF / e-mail para autenticação).</summary>
public interface IEstudanteReadRepository
{
    Task<EstudanteDados?> ObterPorCpfSomenteDigitosAsync(string cpfSomenteDigitos, CancellationToken cancellationToken = default);

    Task<EstudanteDados?> ObterPorEmailNormalizadoAsync(string emailNormalizado, CancellationToken cancellationToken = default);
}
