using MediatR;

namespace HolidayPlanner.Application.Test.Commands.DeleteTestHoliday;

public sealed record DeleteTestHolidayCommand(Guid Id) : IRequest;
