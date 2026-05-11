using HolidayPlanner.Application.Test.Interfaces;
using HolidayPlanner.Domain.Common.Exceptions;
using MediatR;

namespace HolidayPlanner.Application.Test.Commands.UpdateTestHoliday;

public sealed class UpdateTestHolidayCommandHandler(
    ITestHolidayRepository repository) : IRequestHandler<UpdateTestHolidayCommand>
{
    public async Task Handle(
        UpdateTestHolidayCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Test.TestHoliday), request.Id);

        entity.Name = request.Name;
        entity.Destination = request.Destination;
        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;

        await repository.SaveChangesAsync(cancellationToken);
    }
}
