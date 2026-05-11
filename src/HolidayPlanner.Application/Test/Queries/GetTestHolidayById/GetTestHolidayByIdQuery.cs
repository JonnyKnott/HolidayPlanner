using HolidayPlanner.Application.Test.DTOs;
using MediatR;

namespace HolidayPlanner.Application.Test.Queries.GetTestHolidayById;

public sealed record GetTestHolidayByIdQuery(Guid Id) : IRequest<TestHolidayDto>;
