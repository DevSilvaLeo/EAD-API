using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using THEONEEAD.Domain.Entities;

namespace THEONEEAD.Infrastructure.Persistence.Seminario.Configurations;

public class CursoEADConfiguration : IEntityTypeConfiguration<CursoEAD>
{
    public void Configure(EntityTypeBuilder<CursoEAD> builder)
    {
        builder.ToTable("CursoEAD");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Nome).IsRequired().HasMaxLength(500);
        builder.Property(x => x.CursoLegadoId).IsRequired();
        builder.HasIndex(x => x.CursoLegadoId).IsUnique();
        builder.HasMany(c => c.Disciplinas)
            .WithOne(x => x.Curso)
            .HasForeignKey(x => x.CursoEADId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
