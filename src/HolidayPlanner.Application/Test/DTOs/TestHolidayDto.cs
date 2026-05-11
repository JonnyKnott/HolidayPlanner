namespace HolidayPlanner.Application.Test.DTOs;

public record TestHolidayDto(
    Guid Id,
    string Name,
    string Destination,
    DateOnly StartDate,
    DateOnly EndDate,
    DateTimeOffset CreatedOn,
    DateTimeOffset ModifiedOn);
