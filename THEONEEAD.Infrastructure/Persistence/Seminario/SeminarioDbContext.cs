using Microsoft.EntityFrameworkCore;
using THEONEEAD.Domain.Entities;
using THEONEEAD.Infrastructure.Persistence.Seminario.Configurations;
using THEONEEAD.Infrastructure.Persistence.Seminario.Entities;

namespace THEONEEAD.Infrastructure.Persistence.Seminario;

/// <summary>Contexto único do MySQL (base <c>seminario</c>): EAD + cadastro acadêmico.</summary>
public class SeminarioDbContext : DbContext
{
    public SeminarioDbContext(DbContextOptions<SeminarioDbContext> options) : base(options) { }

    public DbSet<CursoEAD> CursosEAD => Set<CursoEAD>();
    public DbSet<DisciplinaEAD> DisciplinasEAD => Set<DisciplinaEAD>();
    public DbSet<Conteudo> Conteudos => Set<Conteudo>();
    public DbSet<Dependencia> Dependencias => Set<Dependencia>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public DbSet<Estudante> Estudantes => Set<Estudante>();
    public DbSet<AlunoCurso> AlunoCursos => Set<AlunoCurso>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CursoEADConfiguration());
        modelBuilder.ApplyConfiguration(new DisciplinaEADConfiguration());
        modelBuilder.ApplyConfiguration(new ConteudoConfiguration());
        modelBuilder.ApplyConfiguration(new DependenciaConfiguration());
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        modelBuilder.ApplyConfiguration(new EstudanteConfiguration());
        modelBuilder.ApplyConfiguration(new AlunoCursoConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
