using FluentAssertions;
using HolidayPlanner.Application.Test.Commands.UpdateTestHoliday;
using HolidayPlanner.Application.Test.Interfaces;
using HolidayPlanner.Domain.Common.Exceptions;
using HolidayPlanner.Domain.Test;
using NSubstitute;

namespace HolidayPlanner.Tests.Test.Handlers;

public sealed class UpdateTestHolidayCommandHandlerTests
{
    private readonly ITestHolidayRepository _repository = Substitute.For<ITestHolidayRepository>();
    private readonly UpdateTestHolidayCommandHandler _sut;

    public UpdateTestHolidayCommandHandlerTests()
    {
        _sut = new UpdateTestHolidayCommandHandler(_repository);
    }

    [Fact]
    public async Task Handle_WhenEntityExists_UpdatesFieldsAndSaves()
    {
        var holiday = new TestHoliday
        {
            Name = "Old Name",
            Destination = TestDestination.Japan,
            StartDate = new DateOnly(2026, 7, 1),
            EndDate = new DateOnly(2026, 7, 14),
        };
        _repository.GetByIdTrackedAsync(holiday.Id, Arg.Any<CancellationToken>()).Returns(holiday);

        var command = new UpdateTestHolidayCommand(
            Id: holiday.Id,
            Name: "New Name",
            Destination: TestDestination.Scotland,
            StartDate: new DateOnly(2026, 8, 1),
            EndDate: new DateOnly(2026, 8, 10));

        await _sut.Handle(command, CancellationToken.None);

        holiday.Name.Should().Be("New Name");
        holiday.Destination.Should().Be(TestDestination.Scotland);
        holiday.StartDate.Should().Be(new DateOnly(2026, 8, 1));
        holiday.EndDate.Should().Be(new DateOnly(2026, 8, 10));
        await _repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenEntityDoesNotExist_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _repository.GetByIdTrackedAsync(id, Arg.Any<CancellationToken>()).Returns((TestHoliday?)null);

        var command = new UpdateTestHolidayCommand(
            Id: id,
            Name: "Name",
            Destination: TestDestination.Mexico,
            StartDate: new DateOnly(2026, 6, 1),
            EndDate: new DateOnly(2026, 6, 10));

        var act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        await _repository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
