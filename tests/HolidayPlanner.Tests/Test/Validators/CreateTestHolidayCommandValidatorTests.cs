using FluentAssertions;
using FluentValidation.TestHelper;
using HolidayPlanner.Application.Test.Commands.CreateTestHoliday;
using HolidayPlanner.Domain.Test;

namespace HolidayPlanner.Tests.Test.Validators;

public sealed class CreateTestHolidayCommandValidatorTests
{
    private readonly CreateTestHolidayCommandValidator _sut = new();

    private static CreateTestHolidayCommand ValidCommand() => new(
        Name: "Summer Holiday",
        Destination: TestDestination.Japan,
        StartDate: new DateOnly(2026, 7, 1),
        EndDate: new DateOnly(2026, 7, 14));

    [Fact]
    public async Task Validate_ValidCommand_PassesValidation()
    {
        var command = ValidCommand();

        var result = await _sut.TestValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Validate_EmptyName_FailsWithNameRequired(string name)
    {
        var command = ValidCommand() with { Name = name };

        var result = await _sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name is required.");
    }

    [Theory]
    [InlineData("")]
    [InlineData("Paris")]
    [InlineData("random")]
    public async Task Validate_InvalidDestination_FailsWithDestinationError(string destination)
    {
        var command = ValidCommand() with { Destination = destination };

        var result = await _sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Destination);
    }

    [Theory]
    [InlineData("Mexico")]
    [InlineData("Japan")]
    [InlineData("New Zealand")]
    [InlineData("Iceland")]
    [InlineData("Scotland")]
    public async Task Validate_ValidDestination_PassesDestinationValidation(string destination)
    {
        var command = ValidCommand() with { Destination = destination };

        var result = await _sut.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Destination);
    }

    [Fact]
    public async Task Validate_EndDateBeforeStartDate_FailsWithDateError()
    {
        var command = ValidCommand() with
        {
            StartDate = new DateOnly(2026, 7, 14),
            EndDate = new DateOnly(2026, 7, 1),
        };

        var result = await _sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.EndDate)
            .WithErrorMessage("End date must be after start date.");
    }

    [Fact]
    public async Task Validate_EndDateSameAsStartDate_FailsWithDateError()
    {
        var date = new DateOnly(2026, 7, 14);
        var command = ValidCommand() with { StartDate = date, EndDate = date };

        var result = await _sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.EndDate)
            .WithErrorMessage("End date must be after start date.");
    }

    [Fact]
    public async Task Validate_EndDateAfterStartDate_PassesDateValidation()
    {
        var command = ValidCommand() with
        {
            StartDate = new DateOnly(2026, 7, 1),
            EndDate = new DateOnly(2026, 7, 2),
        };

        var result = await _sut.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.EndDate);
    }
}
