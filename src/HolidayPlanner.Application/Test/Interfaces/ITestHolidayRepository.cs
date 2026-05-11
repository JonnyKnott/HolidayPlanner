using HolidayPlanner.Domain.Test;

namespace HolidayPlanner.Application.Test.Interfaces;

public interface ITestHolidayRepository
{
    Task<IReadOnlyList<TestHoliday>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Read-only, no change tracking. Use in query handlers.</summary>
    Task<TestHoliday?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Change-tracked. Use in command handlers that mutate the entity.</summary>
    Task<TestHoliday?> GetByIdTrackedAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(TestHoliday entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TestHoliday entity, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
