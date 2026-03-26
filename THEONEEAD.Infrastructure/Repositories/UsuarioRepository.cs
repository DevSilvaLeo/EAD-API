using Microsoft.EntityFrameworkCore;
using THEONEEAD.Domain.Entities;
using THEONEEAD.Domain.Repositories;
using THEONEEAD.Infrastructure.Persistence.Seminario;

namespace THEONEEAD.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly SeminarioDbContext _ctx;
    private readonly IEstudanteReadRepository _estudantes;

    public UsuarioRepository(SeminarioDbContext ctx, IEstudanteReadRepository estudantes)
    {
        _ctx = ctx;
        _estudantes = estudantes;
    }

    public void Adicionar(Usuario usuario) => _ctx.Usuarios.Add(usuario);

    public Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var e = email.Trim().ToLowerInvariant();
        return _ctx.Usuarios.FirstOrDefaultAsync(x => x.Login == e, cancellationToken);
    }

    public async Task<Usuario?> ObterPorCpfNormalizadoAsync(string cpfSomenteDigitos, CancellationToken cancellationToken = default)
    {
        var aluno = await _estudantes.ObterPorCpfSomenteDigitosAsync(cpfSomenteDigitos, cancellationToken);
        if (aluno is null || string.IsNullOrEmpty(aluno.EmailNormalizado))
            return null;

        return await ObterPorEmailAsync(aluno.EmailNormalizado, cancellationToken);
    }

    public Task<Usuario?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default) =>
        _ctx.Usuarios.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}
