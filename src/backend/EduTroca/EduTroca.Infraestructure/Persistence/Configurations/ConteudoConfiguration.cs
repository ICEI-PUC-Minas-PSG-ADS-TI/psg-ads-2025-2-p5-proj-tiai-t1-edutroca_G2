using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduTroca.Infraestructure.Persistence.Configurations;
public class ConteudoConfiguration : IEntityTypeConfiguration<Conteudo>
{
    public void Configure(EntityTypeBuilder<Conteudo> builder)
    {
        builder.ToTable("Conteudos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Titulo).HasMaxLength(150).IsRequired();
        builder.Property(c => c.Descricao).HasMaxLength(500).IsRequired();

        builder.HasDiscriminator(c=>c.Tipo)
               .HasValue<Video>(EConteudoTipo.Video)
               .HasValue<Pergunta>(EConteudoTipo.Pergunta);

        builder.HasOne(c => c.Autor)
               .WithMany(u => u.Conteudos);

        builder.HasMany(c => c.Likes)
            .WithMany(u=>u.Likes)
            .UsingEntity(j => j.ToTable("Likes"));

        builder.HasMany(c => c.Dislikes)
            .WithMany(u => u.Dislikes)
            .UsingEntity(j => j.ToTable("Dislikes"));

        builder.HasQueryFilter(c => !c.Autor!.IsDeleted);
    }
}
