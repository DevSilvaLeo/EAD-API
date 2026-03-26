using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using THEONEEAD.Domain.Entities;

namespace THEONEEAD.Infrastructure.Persistence.Seminario.Configurations;

public class ConteudoConfiguration : IEntityTypeConfiguration<Conteudo>
{
    public void Configure(EntityTypeBuilder<Conteudo> builder)
    {
        builder.ToTable("Conteudo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Titulo).IsRequired().HasMaxLength(500);
        builder.Property(x => x.Tipo).HasConversion<int>();
        builder.Property(x => x.UrlVideo).HasMaxLength(2000);
        builder.Property(x => x.UrlSlides).HasMaxLength(2000);
        builder.Property(x => x.ConteudoTexto).HasMaxLength(8000);
        builder.HasIndex(x => new { x.DisciplinaEADId, x.Ordem });
    }
}
