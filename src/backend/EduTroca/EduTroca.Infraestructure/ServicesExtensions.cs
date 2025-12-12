using EduTroca.Core.Abstractions;
using EduTroca.Infraestructure.Auth;
using EduTroca.Infraestructure.Email;
using EduTroca.Infraestructure.Persistence;
using EduTroca.Infraestructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EduTroca.Infraestructure;
public static class ServicesExtensions
{
    public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SQLServer"));
        });
        services.AddFluentEmail(configuration["Email:SenderEmail"], configuration["Email:Sender"])
            .AddRazorRenderer()
            .AddSmtpSender(configuration["Email:Host"], configuration.GetValue<int>("Email:Port"),
            configuration["Email:Username"], configuration["Email:Password"]);
        services.AddScoped<IEmailService, FluentEmailService>();
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IPasswordHasher, BCryptHasher>();
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IFileService, RootFileService>();

        return services;
    }
}
