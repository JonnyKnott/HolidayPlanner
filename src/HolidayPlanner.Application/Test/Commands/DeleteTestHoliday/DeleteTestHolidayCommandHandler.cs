using HolidayPlanner.Application.Test.Interfaces;
using HolidayPlanner.Domain.Common.Exceptions;
using MediatR;

namespace HolidayPlanner.Application.Test.Commands.DeleteTestHoliday;

public sealed class DeleteTestHolidayCommandHandler(
    ITestHolidayRepository repository) : IRequestHandler<DeleteTestHolidayCommand>
{
    public async Task Handle(
        DeleteTestHolidayCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Test.TestHoliday), request.Id);

        await repository.DeleteAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
