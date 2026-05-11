using HolidayPlanner.Domain.Common;

namespace HolidayPlanner.Domain.Test;

public sealed class TestHoliday : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
