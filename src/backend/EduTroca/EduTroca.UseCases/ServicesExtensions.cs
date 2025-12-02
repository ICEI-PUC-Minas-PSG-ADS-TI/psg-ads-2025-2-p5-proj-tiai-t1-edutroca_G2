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
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        });
        services.AddAuthorizers();
        return services;
    }
    private static IServiceCollection AddAuthorizers(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<IAuthorizer<object>>()
            .AddClasses(classes => classes.AssignableTo(typeof(IAuthorizer<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
        return services;
    }
}
