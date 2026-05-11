using AutoMapper;
using HolidayPlanner.Application.Test.DTOs;
using HolidayPlanner.Application.Test.Interfaces;
using HolidayPlanner.Domain.Common.Exceptions;
using MediatR;

namespace HolidayPlanner.Application.Test.Queries.GetTestHolidayById;

public sealed class GetTestHolidayByIdQueryHandler(
    ITestHolidayRepository repository,
    IMapper mapper) : IRequestHandler<GetTestHolidayByIdQuery, TestHolidayDto>
{
    public async Task<TestHolidayDto> Handle(
        GetTestHolidayByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Test.TestHoliday), request.Id);

        return mapper.Map<TestHolidayDto>(entity);
    }
}
