namespace HolidayPlanner.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset ModifiedOn { get; private set; }

    internal void SetCreatedOn(DateTimeOffset timestamp) => CreatedOn = timestamp;
    internal void SetModifiedOn(DateTimeOffset timestamp) => ModifiedOn = timestamp;
}
