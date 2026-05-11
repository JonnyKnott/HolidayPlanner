using AutoMapper;
using FluentAssertions;
using HolidayPlanner.Application.Test.Interfaces;
using HolidayPlanner.Application.Test.Mappings;
using HolidayPlanner.Application.Test.Queries.GetTestHolidayById;
using HolidayPlanner.Domain.Common.Exceptions;
using HolidayPlanner.Domain.Test;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace HolidayPlanner.Tests.Test.Handlers;

public sealed class GetTestHolidayByIdQueryHandlerTests
{
    private readonly ITestHolidayRepository _repository = Substitute.For<ITestHolidayRepository>();
    private readonly IMapper _mapper;
    private readonly GetTestHolidayByIdQueryHandler _sut;

    public GetTestHolidayByIdQueryHandlerTests()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.AddProfile<TestHolidayProfile>());
        _mapper = services.BuildServiceProvider().GetRequiredService<IMapper>();
        _sut = new GetTestHolidayByIdQueryHandler(_repository, _mapper);
    }

    [Fact]
    public async Task Handle_WhenEntityExists_ReturnsMappedDto()
    {
        var holiday = new TestHoliday
        {
            Name = "My Holiday",
            Destination = TestDestination.NewZealand,
            StartDate = new DateOnly(2026, 11, 1),
            EndDate = new DateOnly(2026, 11, 14),
        };
        _repository.GetByIdAsync(holiday.Id, Arg.Any<CancellationToken>()).Returns(holiday);

        var result = await _sut.Handle(new GetTestHolidayByIdQuery(holiday.Id), CancellationToken.None);

        result.Id.Should().Be(holiday.Id);
        result.Name.Should().Be("My Holiday");
        result.Destination.Should().Be(TestDestination.NewZealand);
    }

    [Fact]
    public async Task Handle_WhenEntityDoesNotExist_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((TestHoliday?)null);

        var act = async () => await _sut.Handle(new GetTestHolidayByIdQuery(id), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
