namespace HolidayPlanner.Domain.Test;

public static class TestDestination
{
    public const string Mexico = "Mexico";
    public const string Japan = "Japan";
    public const string NewZealand = "New Zealand";
    public const string Iceland = "Iceland";
    public const string Scotland = "Scotland";

    public static readonly IReadOnlyCollection<string> All =
    [
        Mexico,
        Japan,
        NewZealand,
        Iceland,
        Scotland,
    ];
}
