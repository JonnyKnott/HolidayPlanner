using HolidayPlanner.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HolidayPlanner.IntegrationTests.Test;

public sealed class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private static string SaPassword =>
        Environment.GetEnvironmentVariable("SA_PASSWORD") ?? "YourStrong@Passw0rd";

    private static string TestConnectionString =>
        $"Server=localhost,1433;Database=HolidayPlannerIntegrationTests;User Id=sa;Password={SaPassword};TrustServerCertificate=True;";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        // Override the connection string before the app's own configuration runs.
        builder.UseSetting("ConnectionStrings:DefaultConnection", TestConnectionString);
    }

    public async Task InitializeAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<HolidayPlannerDbContext>();
        await db.Database.MigrateAsync();
    }

    public new async Task DisposeAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<HolidayPlannerDbContext>();
        await db.Database.EnsureDeletedAsync();
        await base.DisposeAsync();
    }
}
