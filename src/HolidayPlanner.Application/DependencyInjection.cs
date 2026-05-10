using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace HolidayPlanner.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddAutoMapper(cfg => { }, typeof(DependencyInjection).Assembly);
        return services;
    }
}
