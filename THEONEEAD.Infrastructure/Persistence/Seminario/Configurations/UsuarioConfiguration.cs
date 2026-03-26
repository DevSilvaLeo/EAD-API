using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using THEONEEAD.Domain.Entities;

namespace THEONEEAD.Infrastructure.Persistence.Seminario.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios_ead");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Status).HasColumnName("status");
        builder.Property(x => x.Login).IsRequired().HasMaxLength(150).HasColumnName("login");
        builder.HasIndex(x => x.Login).IsUnique();
        builder.Property(x => x.SenhaHash).IsRequired().HasMaxLength(1000).HasColumnName("senha");
        builder.Property(x => x.Perfil).HasColumnName("perfil").HasConversion<byte>();
        builder.Property(x => x.RecuperaSenha).HasMaxLength(1000).HasColumnName("recupera_senha");
        builder.Property(x => x.PrimeiroAcesso).HasMaxLength(1000).HasColumnName("primeiro_acesso");
    }
}
