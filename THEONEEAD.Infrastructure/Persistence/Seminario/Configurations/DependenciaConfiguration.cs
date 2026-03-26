using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using THEONEEAD.Domain.Entities;

namespace THEONEEAD.Infrastructure.Persistence.Seminario.Configurations;

public class DependenciaConfiguration : IEntityTypeConfiguration<Dependencia>
{
    public void Configure(EntityTypeBuilder<Dependencia> builder)
    {
        builder.ToTable("Dependencia");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Tipo).HasConversion<int>();
        builder.HasIndex(x => new { x.OrigemId, x.DestinoId, x.Tipo }).IsUnique();
    }
}
