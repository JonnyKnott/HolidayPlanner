using HolidayPlanner.Domain.Test;

namespace HolidayPlanner.Application.Test.Interfaces;

public interface ITestHolidayRepository
{
    Task<IReadOnlyList<TestHoliday>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestHoliday?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(TestHoliday entity, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeleteAsync(TestHoliday entity, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
