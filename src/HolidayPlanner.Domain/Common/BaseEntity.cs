namespace HolidayPlanner.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    /// <summary>Set and maintained by the EF Core save interceptor in Infrastructure.</summary>
    public DateTimeOffset CreatedOn { get; private set; }

    /// <summary>Set and maintained by the EF Core save interceptor in Infrastructure.</summary>
    public DateTimeOffset ModifiedOn { get; private set; }
}
