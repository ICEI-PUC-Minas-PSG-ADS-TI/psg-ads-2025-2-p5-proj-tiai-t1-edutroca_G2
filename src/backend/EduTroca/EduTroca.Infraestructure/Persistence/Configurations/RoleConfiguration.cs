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

        builder.Property(r => r.Nome)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(r => r.Code)
            .IsUnique();


        builder.HasData(
            new
            {
                Id = Guid.Parse("1a4ab49c-5036-4316-b2f6-c2e8fac39705"),
                Code = ERole.Admin,
                Nome = nameof(ERole.Admin)
            },
            new
            {
                Id = Guid.Parse("c999156a-c73e-49b2-9789-ed7e28f1afe7"),
                Code = ERole.User,
                Nome = nameof(ERole.User)
            },
            new
            {
                Id = Guid.Parse("7ae60301-fa05-4512-9361-387262ea60b4"),
                Code = ERole.Owner,
                Nome = nameof(ERole.Owner)
            }
        );
    }
}
