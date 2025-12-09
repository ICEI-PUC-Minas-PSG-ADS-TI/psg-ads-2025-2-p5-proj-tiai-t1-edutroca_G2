using EduTroca.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduTroca.Infraestructure.Persistence.Configurations;
public class ComentarioConfiguration : IEntityTypeConfiguration<Comentario>
{
    public void Configure(EntityTypeBuilder<Comentario> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Texto).HasMaxLength(3000).IsRequired();

        builder.HasOne(c => c.Autor)
               .WithMany(u => u.Comentarios);

        builder.HasOne(c => c.Conteudo)
               .WithMany(c => c.Comentarios)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(c => !c.Autor!.IsDeleted);
    }
}