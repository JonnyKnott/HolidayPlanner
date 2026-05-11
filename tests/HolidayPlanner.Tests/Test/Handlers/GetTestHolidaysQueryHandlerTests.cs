using AutoMapper;
using FluentAssertions;
using HolidayPlanner.Application.Test.DTOs;
using HolidayPlanner.Application.Test.Interfaces;
using HolidayPlanner.Application.Test.Mappings;
using HolidayPlanner.Application.Test.Queries.GetTestHolidays;
using HolidayPlanner.Domain.Test;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace HolidayPlanner.Tests.Test.Handlers;

public sealed class GetTestHolidaysQueryHandlerTests
{
    private readonly ITestHolidayRepository _repository = Substitute.For<ITestHolidayRepository>();
    private readonly IMapper _mapper;
    private readonly GetTestHolidaysQueryHandler _sut;

    public GetTestHolidaysQueryHandlerTests()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.AddProfile<TestHolidayProfile>());
        _mapper = services.BuildServiceProvider().GetRequiredService<IMapper>();
        _sut = new GetTestHolidaysQueryHandler(_repository, _mapper);
    }

    [Fact]
    public async Task Handle_WhenHolidaysExist_ReturnsMappedDtos()
    {
        var holidays = new List<TestHoliday>
        {
            new() { Name = "Trip A", Destination = TestDestination.Japan, StartDate = new DateOnly(2026, 7, 1), EndDate = new DateOnly(2026, 7, 14) },
            new() { Name = "Trip B", Destination = TestDestination.Mexico, StartDate = new DateOnly(2026, 8, 1), EndDate = new DateOnly(2026, 8, 7) },
        };
        _repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(holidays);

        var result = await _sut.Handle(new GetTestHolidaysQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
        result.Should().ContainSingle(x => x.Name == "Trip A" && x.Destination == TestDestination.Japan);
        result.Should().ContainSingle(x => x.Name == "Trip B" && x.Destination == TestDestination.Mexico);
    }

    [Fact]
    public async Task Handle_WhenNoHolidaysExist_ReturnsEmptyList()
    {
        _repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<TestHoliday>());

        var result = await _sut.Handle(new GetTestHolidaysQuery(), CancellationToken.None);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_MapsAllFieldsCorrectly()
    {
        var holiday = new TestHoliday
        {
            Name = "Full Map Test",
            Destination = TestDestination.Scotland,
            StartDate = new DateOnly(2026, 9, 1),
            EndDate = new DateOnly(2026, 9, 10),
        };
        _repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<TestHoliday> { holiday });

        var result = await _sut.Handle(new GetTestHolidaysQuery(), CancellationToken.None);

        var dto = result.Single();
        dto.Id.Should().Be(holiday.Id);
        dto.Name.Should().Be("Full Map Test");
        dto.Destination.Should().Be(TestDestination.Scotland);
        dto.StartDate.Should().Be(new DateOnly(2026, 9, 1));
        dto.EndDate.Should().Be(new DateOnly(2026, 9, 10));
    }
}
