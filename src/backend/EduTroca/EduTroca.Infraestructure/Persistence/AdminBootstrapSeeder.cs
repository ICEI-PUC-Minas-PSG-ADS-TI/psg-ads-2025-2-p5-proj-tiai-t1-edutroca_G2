using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EduTroca.Infraestructure.Persistence;
public static class OwnerBootstrapSeeder
{
    public static async Task SeedAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var config = services.GetRequiredService<IConfiguration>();

        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            var passwordHasher = services.GetRequiredService<IPasswordHasher>();

            if (await context.Set<Usuario>().AnyAsync())
                return;

            var email = config["SeedSettings:OwnerEmail"];
            var senha = config["SeedSettings:OwnerPassword"];
            var ownerName = "Owner";

            var passwordHash = passwordHasher.HashPassword(senha);

            var ownerRole = await context.Set<Role>()
                .FirstOrDefaultAsync(r => r.Id == (int)ERole.Owner);

            if (ownerRole == null)
            {
                ownerRole = new Role((int)ERole.Owner, "Owner");
                context.Set<Role>().Attach(ownerRole);
            }

            var ownerUser = new Usuario(
                ownerName,
                email,
                passwordHash,
                DateTime.UtcNow.AddYears(1),
                [ownerRole]
            );

            ownerUser.ConfirmEmail();

            await context.Set<Usuario>().AddAsync(ownerUser);
            await context.SaveChangesAsync();

            Console.WriteLine("Seed: Usuário Owner criado com sucesso.");
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Usuario>>();
            logger.LogError(ex, "Ocorreu um erro ao popular o banco de dados.");
        }
    }
}
