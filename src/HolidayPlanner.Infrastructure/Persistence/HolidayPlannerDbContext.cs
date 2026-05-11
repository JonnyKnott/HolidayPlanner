using HolidayPlanner.Domain.Test;
using Microsoft.EntityFrameworkCore;

namespace HolidayPlanner.Infrastructure.Persistence;

public sealed class HolidayPlannerDbContext(DbContextOptions<HolidayPlannerDbContext> options)
    : DbContext(options)
{
    public DbSet<TestHoliday> TestHolidays => Set<TestHoliday>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HolidayPlannerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
