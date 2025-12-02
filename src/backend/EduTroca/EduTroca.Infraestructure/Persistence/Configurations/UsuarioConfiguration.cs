using EduTroca.Core.Entities.UsuarioAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduTroca.Infraestructure.Persistence.Configurations;
public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(u => u.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.SenhaHash)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(u => u.Bio)
            .HasMaxLength(300)
            .IsRequired(false);

        builder.Property(u => u.CaminhoImagem)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.OwnsMany(u => u.RefreshTokens, rtBuilder =>
        {
            rtBuilder.WithOwner().HasForeignKey("UsuarioId");
            rtBuilder.Property(rt => rt.Id).ValueGeneratedNever();
            rtBuilder.HasKey(rt => rt.Id);
            rtBuilder.ToTable("RefreshTokens");
        });

        builder.OwnsOne(u => u.EmailConfirmationCode, eccBuilder =>
        {
            eccBuilder.WithOwner().HasForeignKey("UsuarioId");
            eccBuilder.Property(ecc => ecc.Id).ValueGeneratedNever();
            eccBuilder.HasKey(ecc => ecc.Id);
            eccBuilder.ToTable("EmailConfirmationCode");
        });

        builder.HasMany(u => u.CategoriasDeInteresse)
            .WithMany()
            .UsingEntity("UsuarioCategorias");

        builder.HasMany(u => u.Roles)
               .WithMany()
               .UsingEntity(j => j.ToTable("UsuarioRoles"));
    }
}
