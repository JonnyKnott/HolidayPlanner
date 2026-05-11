using FluentAssertions;
using HolidayPlanner.Application.Test.Commands.DeleteTestHoliday;
using HolidayPlanner.Application.Test.Interfaces;
using HolidayPlanner.Domain.Common.Exceptions;
using HolidayPlanner.Domain.Test;
using NSubstitute;

namespace HolidayPlanner.Tests.Test.Handlers;

public sealed class DeleteTestHolidayCommandHandlerTests
{
    private readonly ITestHolidayRepository _repository = Substitute.For<ITestHolidayRepository>();
    private readonly DeleteTestHolidayCommandHandler _sut;

    public DeleteTestHolidayCommandHandlerTests()
    {
        _sut = new DeleteTestHolidayCommandHandler(_repository);
    }

    [Fact]
    public async Task Handle_WhenEntityExists_DeletesAndSaves()
    {
        var holiday = new TestHoliday
        {
            Name = "To Delete",
            Destination = TestDestination.Iceland,
            StartDate = new DateOnly(2026, 3, 1),
            EndDate = new DateOnly(2026, 3, 7),
        };
        _repository.GetByIdTrackedAsync(holiday.Id, Arg.Any<CancellationToken>()).Returns(holiday);

        await _sut.Handle(new DeleteTestHolidayCommand(holiday.Id), CancellationToken.None);

        await _repository.Received(1).DeleteAsync(holiday, Arg.Any<CancellationToken>());
        await _repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenEntityDoesNotExist_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _repository.GetByIdTrackedAsync(id, Arg.Any<CancellationToken>()).Returns((TestHoliday?)null);

        var act = async () => await _sut.Handle(new DeleteTestHolidayCommand(id), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        await _repository.DidNotReceive().DeleteAsync(Arg.Any<TestHoliday>(), Arg.Any<CancellationToken>());
        await _repository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
