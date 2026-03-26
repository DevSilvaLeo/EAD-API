using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Domain.Entities;
using THEONEEAD.Domain.Repositories;
using THEONEEAD.Infrastructure.Persistence.Seminario;

namespace THEONEEAD.Infrastructure.Hosting;

public class SeminarioDatabaseInitializer : IHostedService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<SeminarioDatabaseInitializer> _logger;

    public SeminarioDatabaseInitializer(IServiceProvider services, ILogger<SeminarioDatabaseInitializer> logger)
    {
        _services = services;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SeminarioDbContext>();
       // await ctx.Database.MigrateAsync(cancellationToken);
       /*
        if (!await ctx.Usuarios.AnyAsync(cancellationToken))
        {
            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
            var usuarios = scope.ServiceProvider.GetRequiredService<IUsuarioRepository>();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorkSeminario>();

            var admin = Usuario.CriarAdministrador("admin@ead.com", "Administrador", hasher.Hash("Senha@123"));
            usuarios.Adicionar(admin);

            await uow.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Seed: admin@ead.com / Senha@123 (usuarios_ead em MySQL seminario).");
        }
       
        if (!await ctx.CursosEAD.AnyAsync(cancellationToken))
        {
            ctx.CursosEAD.Add(CursoEAD.Criar(1, "Curso EAD Demo", sequencial: false));
            await ctx.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Seed curso EAD demo (CursoLegadoId=1).");
        }
       */
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
