using HolidayPlanner.Application.Test.DTOs;
using MediatR;

namespace HolidayPlanner.Application.Test.Queries.GetTestHolidays;

public sealed record GetTestHolidaysQuery : IRequest<IReadOnlyList<TestHolidayDto>>;
