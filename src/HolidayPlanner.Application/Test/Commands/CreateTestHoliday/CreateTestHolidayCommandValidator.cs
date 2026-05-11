using FluentValidation;
using HolidayPlanner.Domain.Test;

namespace HolidayPlanner.Application.Test.Commands.CreateTestHoliday;

public sealed class CreateTestHolidayCommandValidator : AbstractValidator<CreateTestHolidayCommand>
{
    public CreateTestHolidayCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(x => x.Destination)
            .NotEmpty()
            .WithMessage("Destination is required.")
            .Must(d => TestDestination.All.Contains(d))
            .WithMessage($"Destination must be one of: {string.Join(", ", TestDestination.All)}.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date.");
    }
}
