using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using THEONEEAD.Infrastructure.Persistence.Seminario.Entities;

namespace THEONEEAD.Infrastructure.Persistence.Seminario.Configurations;

public class AlunoCursoConfiguration : IEntityTypeConfiguration<AlunoCurso>
{
    public void Configure(EntityTypeBuilder<AlunoCurso> builder)
    {
        builder.ToTable("legado_aluno_curso");
        builder.HasKey(x => new { x.AlunoId, x.CursoId });
    }
}
