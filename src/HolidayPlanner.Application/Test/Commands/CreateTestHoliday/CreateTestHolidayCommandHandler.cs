using HolidayPlanner.Application.Test.Interfaces;
using HolidayPlanner.Domain.Test;
using MediatR;

namespace HolidayPlanner.Application.Test.Commands.CreateTestHoliday;

public sealed class CreateTestHolidayCommandHandler(
    ITestHolidayRepository repository) : IRequestHandler<CreateTestHolidayCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateTestHolidayCommand request,
        CancellationToken cancellationToken)
    {
        var entity = new TestHoliday
        {
            Name = request.Name,
            Destination = request.Destination,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
        };

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
