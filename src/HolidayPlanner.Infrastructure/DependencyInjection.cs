using Microsoft.Extensions.DependencyInjection;

namespace HolidayPlanner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Data store registrations added per feature
        return services;
    }
}
