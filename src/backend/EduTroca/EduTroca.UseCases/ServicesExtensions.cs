using EduTroca.UseCases.Common.Authorization;
using EduTroca.UseCases.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EduTroca.UseCases;
public static class ServicesExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        });
        services.AddAuthorization(assembly);
        return services;
    }
    private static IServiceCollection AddAuthorization(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IAuthorizationRequirement<>))
                .Where(type => !type.IsAbstract && !type.IsInterface))
            .AsSelf()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IAuthorizer<>))
                .Where(type => !type.IsAbstract && !type.IsInterface))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
        return services;
    }
}
