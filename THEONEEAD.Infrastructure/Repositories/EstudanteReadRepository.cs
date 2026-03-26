using Microsoft.EntityFrameworkCore;
using THEONEEAD.Domain.Entities;
using THEONEEAD.Domain.Estudantes;
using THEONEEAD.Domain.Repositories;
using THEONEEAD.Infrastructure.Persistence.Seminario;
using THEONEEAD.Infrastructure.Persistence.Seminario.Entities;

namespace THEONEEAD.Infrastructure.Repositories;

public class EstudanteReadRepository : IEstudanteReadRepository
{
    private readonly SeminarioDbContext _ctx;

    public EstudanteReadRepository(SeminarioDbContext ctx) => _ctx = ctx;

    public async Task<EstudanteDados?> ObterPorCpfSomenteDigitosAsync(string cpfSomenteDigitos, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(cpfSomenteDigitos))
            return null;

        // 1) CPF já armazenado só com dígitos
        var row = await _ctx.Estudantes.AsNoTracking()
            .Where(x => x.Cpf != null)
            .FirstOrDefaultAsync(x => x.Cpf == cpfSomenteDigitos, cancellationToken);

        if (row is not null)
            return ParaDados(row);

        // 2) CPF com máscara/pontuação no banco — compara apenas dígitos (MySQL 8+ / MariaDB 10.0.5+)
        row = await _ctx.Estudantes
            .FromSqlInterpolated(
                $"""
                SELECT id, cpf, email, nome FROM estudantes
                WHERE REGEXP_REPLACE(COALESCE(cpf, ''), '[^0-9]', '') = {cpfSomenteDigitos}
                LIMIT 1
                """)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return row is null ? null : ParaDados(row);
    }

    public async Task<EstudanteDados?> ObterPorEmailNormalizadoAsync(string emailNormalizado, CancellationToken cancellationToken = default)
    {
        var e = emailNormalizado.Trim().ToLowerInvariant();
        var row = await _ctx.Estudantes.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email != null && x.Email.ToLower() == e, cancellationToken);

        return row is null ? null : ParaDados(row);
    }

    private static EstudanteDados ParaDados(Estudante row)
    {
        var email = row.Email?.Trim() ?? "";
        var emailN = string.IsNullOrEmpty(email) ? "" : email.ToLowerInvariant();
        var cpf = CpfUtil.Normalizar(row.Cpf);
        return new EstudanteDados(row.Id, emailN, row.Nome?.Trim() ?? "Aluno", cpf);
    }
}
