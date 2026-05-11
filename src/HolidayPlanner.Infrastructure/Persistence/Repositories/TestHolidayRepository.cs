using HolidayPlanner.Application.Test.Interfaces;
using HolidayPlanner.Domain.Test;
using Microsoft.EntityFrameworkCore;

namespace HolidayPlanner.Infrastructure.Persistence.Repositories;

internal sealed class TestHolidayRepository(HolidayPlannerDbContext dbContext) : ITestHolidayRepository
{
    public async Task<IReadOnlyList<TestHoliday>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.TestHolidays
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<TestHoliday?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.TestHolidays
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddAsync(TestHoliday entity, CancellationToken cancellationToken = default)
    {
        await dbContext.TestHolidays.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.TestHolidays
            .AsNoTracking()
            .AnyAsync(x => x.Id == id, cancellationToken);
    }

    public Task DeleteAsync(TestHoliday entity, CancellationToken cancellationToken = default)
    {
        dbContext.TestHolidays.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
