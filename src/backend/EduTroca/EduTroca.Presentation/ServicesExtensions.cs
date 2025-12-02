using EduTroca.Core.Abstractions;
using EduTroca.Presentation.Common;
using Microsoft.Extensions.DependencyInjection;

namespace EduTroca.Presentation;
public static class ServicesExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, HttpCurrentUserService>();
        return services;
    }
}
