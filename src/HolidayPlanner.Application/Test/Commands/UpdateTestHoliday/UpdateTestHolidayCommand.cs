using MediatR;

namespace HolidayPlanner.Application.Test.Commands.UpdateTestHoliday;

public sealed record UpdateTestHolidayCommand(
    Guid Id,
    string Name,
    string Destination,
    DateOnly StartDate,
    DateOnly EndDate) : IRequest;
