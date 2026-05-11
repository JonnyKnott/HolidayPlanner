using FluentAssertions;
using HolidayPlanner.Application.Test.Commands.CreateTestHoliday;
using HolidayPlanner.Application.Test.Interfaces;
using HolidayPlanner.Domain.Test;
using NSubstitute;

namespace HolidayPlanner.Tests.Test.Handlers;

public sealed class CreateTestHolidayCommandHandlerTests
{
    private readonly ITestHolidayRepository _repository = Substitute.For<ITestHolidayRepository>();
    private readonly CreateTestHolidayCommandHandler _sut;

    public CreateTestHolidayCommandHandlerTests()
    {
        _sut = new CreateTestHolidayCommandHandler(_repository);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsEntityAndReturnsGuid()
    {
        var command = new CreateTestHolidayCommand(
            Name: "Beach Break",
            Destination: TestDestination.Mexico,
            StartDate: new DateOnly(2026, 6, 1),
            EndDate: new DateOnly(2026, 6, 14));

        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().NotBeEmpty();
        await _repository.Received(1).AddAsync(
            Arg.Is<TestHoliday>(h =>
                h.Name == "Beach Break" &&
                h.Destination == TestDestination.Mexico &&
                h.StartDate == new DateOnly(2026, 6, 1) &&
                h.EndDate == new DateOnly(2026, 6, 14)),
            Arg.Any<CancellationToken>());
        await _repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnedIdMatchesCreatedEntity()
    {
        Guid? capturedId = null;
        await _repository.AddAsync(
            Arg.Do<TestHoliday>(h => capturedId = h.Id),
            Arg.Any<CancellationToken>());

        var command = new CreateTestHolidayCommand(
            Name: "Trip",
            Destination: TestDestination.Iceland,
            StartDate: new DateOnly(2026, 1, 5),
            EndDate: new DateOnly(2026, 1, 12));

        var result = await _sut.Handle(command, CancellationToken.None);

        capturedId.Should().NotBeNull();
        result.Should().Be(capturedId!.Value);
    }
}
