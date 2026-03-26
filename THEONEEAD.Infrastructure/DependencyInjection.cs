using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql;
using THEONEEAD.Application.Auth;
using THEONEEAD.Application.Common.Interfaces;
using THEONEEAD.Domain.Repositories;
using THEONEEAD.Infrastructure.Email;
using THEONEEAD.Infrastructure.Hosting;
using THEONEEAD.Infrastructure.Identity;
using THEONEEAD.Infrastructure.Mongo;
using THEONEEAD.Infrastructure.Persistence.Seminario;
using THEONEEAD.Infrastructure.Repositories;

namespace THEONEEAD.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<MongoDbOptions>(configuration.GetSection(MongoDbOptions.SectionName));
        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));

        var mysqlConn = configuration.GetConnectionString("Seminario")
            ?? configuration.GetConnectionString("MySql");
        if (string.IsNullOrWhiteSpace(mysqlConn))
            throw new InvalidOperationException("Configure ConnectionStrings:Seminario (MySQL, base seminario).");

        var serverVersion = configuration["Seminario:ServerVersion"] ?? "8.0.36-mysql";
        services.AddDbContext<SeminarioDbContext>(options =>
            options.UseMySql(mysqlConn, ServerVersion.Parse(serverVersion)));

        services.AddScoped<IUnitOfWorkSeminario, UnitOfWorkSeminario>();
        services.AddScoped<ICursoEADRepository, CursoEADRepository>();
        services.AddScoped<IDisciplinaEADRepository, DisciplinaEADRepository>();
        services.AddScoped<IConteudoRepository, ConteudoRepository>();
        services.AddScoped<IDependenciaRepository, DependenciaRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IAlunoCursoReadRepository, AlunoCursoReadRepository>();
        services.AddScoped<IEstudanteReadRepository, EstudanteReadRepository>();

        services.AddSingleton<IAlunoCursoProgressoRepository, AlunoCursoProgressoRepository>();
        services.AddSingleton<IForumRepository, ForumRepository>();
        services.AddSingleton<ITarefaEntregaRepository, TarefaEntregaRepository>();

        services.AddSingleton<IPasswordHasher, PasswordHasherService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<ICodigoAcessoGerador, CodigoAcessoGerador>();
        services.AddSingleton<ICodigoVerificacaoStore, CodigoVerificacaoMemoryStore>();
        services.AddSingleton<IPortalAlunoLeituraRepository, PortalAlunoMongoRepository>();
        services.AddScoped<IRecuperacaoSenhaNotificador, RecuperacaoSenhaNotificador>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, HttpContextCurrentUserService>();

        services.AddHostedService<SeminarioDatabaseInitializer>();

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app) => app;
}
