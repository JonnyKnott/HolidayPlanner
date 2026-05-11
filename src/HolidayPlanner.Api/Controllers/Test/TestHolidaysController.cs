using Asp.Versioning;
using HolidayPlanner.Application.Test.Commands.CreateTestHoliday;
using HolidayPlanner.Application.Test.Commands.DeleteTestHoliday;
using HolidayPlanner.Application.Test.Commands.UpdateTestHoliday;
using HolidayPlanner.Application.Test.DTOs;
using HolidayPlanner.Application.Test.Queries.GetTestHolidayById;
using HolidayPlanner.Application.Test.Queries.GetTestHolidays;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HolidayPlanner.Api.Controllers.Test;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/test/holidays")]
public sealed class TestHolidaysController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TestHolidayDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetTestHolidaysQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TestHolidayDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetTestHolidayByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTestHolidayCommand command,
        CancellationToken cancellationToken)
    {
        var id = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateTestHolidayRequest body,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTestHolidayCommand(
            id,
            body.Name,
            body.Destination,
            body.StartDate,
            body.EndDate);

        await sender.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteTestHolidayCommand(id), cancellationToken);
        return NoContent();
    }
}

public sealed record UpdateTestHolidayRequest(
    string Name,
    string Destination,
    DateOnly StartDate,
    DateOnly EndDate);
