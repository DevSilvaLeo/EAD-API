using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using THEONEEAD.Infrastructure.Persistence.Seminario.Entities;

namespace THEONEEAD.Infrastructure.Persistence.Seminario.Configurations;

public class EstudanteConfiguration : IEntityTypeConfiguration<Estudante>
{
    public void Configure(EntityTypeBuilder<Estudante> builder)
    {
        builder.ToTable("estudantes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Cpf).HasColumnName("cpf").HasMaxLength(32);
        builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(320);
        builder.Property(x => x.Nome).HasColumnName("nome").HasMaxLength(300);
    }
}
