using THEONEEAD.Domain.Entities;

namespace THEONEEAD.Domain.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<Usuario?> ObterPorCpfNormalizadoAsync(string cpfSomenteDigitos, CancellationToken cancellationToken = default);

    Task<Usuario?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);

    void Adicionar(Usuario usuario);
}
