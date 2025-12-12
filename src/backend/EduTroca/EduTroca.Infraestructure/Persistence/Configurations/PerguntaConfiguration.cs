using EduTroca.Core.Entities.ConteudoAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduTroca.Infraestructure.Persistence.Configurations;
public class PerguntaConfiguration : IEntityTypeConfiguration<Pergunta>
{
    public void Configure(EntityTypeBuilder<Pergunta> builder)
    {
        builder.Property(p => p.TextoCompleto).HasColumnType("TEXT");
    }
}
