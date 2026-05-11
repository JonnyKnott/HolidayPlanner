using HolidayPlanner.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HolidayPlanner.Infrastructure.Persistence.Interceptors;

public sealed class AuditInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var now = DateTimeOffset.UtcNow;

        foreach (var entry in eventData.Context.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State is EntityState.Added)
            {
                entry.Property(nameof(BaseEntity.CreatedOn)).CurrentValue = now;
                entry.Property(nameof(BaseEntity.ModifiedOn)).CurrentValue = now;
            }
            else if (entry.State is EntityState.Modified)
            {
                entry.Property(nameof(BaseEntity.ModifiedOn)).CurrentValue = now;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
