using MediatR;

namespace HolidayPlanner.Application.Test.Commands.CreateTestHoliday;

public sealed record CreateTestHolidayCommand(
    string Name,
    string Destination,
    DateOnly StartDate,
    DateOnly EndDate) : IRequest<Guid>;
