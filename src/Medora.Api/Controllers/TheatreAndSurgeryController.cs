using Medora.Application.Common.Models;
using Medora.Application.Features.TheatreAndSurgery.Commands.ScheduleSurgery;
using Medora.Application.Features.TheatreAndSurgery.Commands.UpdateSurgeryStatus;
using Medora.Application.Features.TheatreAndSurgery.Queries.GetSurgeryCases;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/theatre-and-surgery")]
public sealed class TheatreAndSurgeryController(
    IGetSurgeryCasesQueryHandler getSurgeryCasesQueryHandler,
    IScheduleSurgeryCommandHandler scheduleSurgeryCommandHandler,
    IUpdateSurgeryStatusCommandHandler updateSurgeryStatusCommandHandler) : ControllerBase
{
    [HttpGet("cases")]
    [HttpGet("/api/operations/surgery-cases")]
    public async Task<ActionResult<ApiResponse<object>>> GetSurgeryCases([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var cases = await getSurgeryCasesQueryHandler.Handle(new GetSurgeryCasesQuery(tenantCode), cancellationToken);
        return Ok(ApiResponse<object>.Ok(cases));
    }

    [HttpPost("cases")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> ScheduleSurgery(
        [FromBody] ScheduleSurgeryCommand command,
        CancellationToken cancellationToken)
    {
        var result = await scheduleSurgeryCommandHandler.Handle(command, cancellationToken);
        return Created($"/api/theatre-and-surgery/cases/{result.Id}", ApiResponse<CommandResult>.Ok(result, "Surgery scheduled successfully."));
    }

    [HttpPatch("cases/{id:guid}/status")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> UpdateStatus(
        Guid id,
        [FromBody] UpdateSurgeryStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await updateSurgeryStatusCommandHandler.Handle(new UpdateSurgeryStatusCommand(id, request.Status), cancellationToken);
        return Ok(ApiResponse<CommandResult>.Ok(result, "Surgery status updated successfully."));
    }
}

public sealed record UpdateSurgeryStatusRequest(string Status);



