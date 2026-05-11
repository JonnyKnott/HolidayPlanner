using AutoMapper;
using HolidayPlanner.Application.Test.DTOs;
using HolidayPlanner.Application.Test.Interfaces;
using MediatR;

namespace HolidayPlanner.Application.Test.Queries.GetTestHolidays;

public sealed class GetTestHolidaysQueryHandler(
    ITestHolidayRepository repository,
    IMapper mapper) : IRequestHandler<GetTestHolidaysQuery, IReadOnlyList<TestHolidayDto>>
{
    public async Task<IReadOnlyList<TestHolidayDto>> Handle(
        GetTestHolidaysQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<TestHolidayDto>>(entities);
    }
}
