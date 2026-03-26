using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using THEONEEAD.Domain.Entities;

namespace THEONEEAD.Infrastructure.Persistence.Seminario.Configurations;

public class DisciplinaEADConfiguration : IEntityTypeConfiguration<DisciplinaEAD>
{
    public void Configure(EntityTypeBuilder<DisciplinaEAD> builder)
    {
        builder.ToTable("DisciplinaEAD");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Nome).IsRequired().HasMaxLength(500);
        builder.HasIndex(x => new { x.CursoEADId, x.Ordem });
        builder.HasMany(d => d.Conteudos)
            .WithOne(c => c.Disciplina)
            .HasForeignKey(c => c.DisciplinaEADId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
