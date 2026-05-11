namespace HolidayPlanner.Api.Models.Test;

public sealed record UpdateTestHolidayRequest(
    string Name,
    string Destination,
    DateOnly StartDate,
    DateOnly EndDate);
