using HolidayPlanner.Application.Test.Interfaces;
using HolidayPlanner.Infrastructure.Persistence;
using HolidayPlanner.Infrastructure.Persistence.Interceptors;
using HolidayPlanner.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HolidayPlanner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<AuditInterceptor>();

        services.AddDbContext<HolidayPlannerDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<AuditInterceptor>();
            options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .AddInterceptors(interceptor);
        });

        services.AddScoped<ITestHolidayRepository, TestHolidayRepository>();

        return services;
    }
}
