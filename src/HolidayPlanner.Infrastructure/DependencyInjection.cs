using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HolidayPlanner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        _ = configuration; // Data store registrations added per feature
        return services;
    }
}
