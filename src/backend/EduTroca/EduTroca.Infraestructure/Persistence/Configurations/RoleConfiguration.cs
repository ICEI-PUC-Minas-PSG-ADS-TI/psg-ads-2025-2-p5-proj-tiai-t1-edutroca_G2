using EduTroca.Core.Entities;
using EduTroca.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduTroca.Infraestructure.Persistence.Configurations;
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id).ValueGeneratedNever();

        builder.Property(r => r.Nome)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasData(
            new Role((int)ERole.User, nameof(ERole.User)),
            new Role((int)ERole.Admin, nameof(ERole.Admin)),
            new Role((int)ERole.Owner, nameof(ERole.Owner))
        );
    }
}
