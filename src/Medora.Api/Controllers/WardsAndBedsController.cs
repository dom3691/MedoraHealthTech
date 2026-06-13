using Medora.Application.Common.Models;
using Medora.Application.Features.WardsAndBeds.Commands.AllocateBed;
using Medora.Application.Features.WardsAndBeds.Commands.UpdateBedStatus;
using Medora.Application.Features.WardsAndBeds.Queries.GetBeds;
using Medora.Application.Features.WardsAndBeds.Queries.GetWards;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/wards-and-beds")]
public sealed class WardsAndBedsController(
    IGetWardsQueryHandler getWardsQueryHandler,
    IGetBedsQueryHandler getBedsQueryHandler,
    IAllocateBedCommandHandler allocateBedCommandHandler,
    IUpdateBedStatusCommandHandler updateBedStatusCommandHandler) : ControllerBase
{
    [HttpGet("wards")]
    [HttpGet("/api/operations/wards")]
    public async Task<ActionResult<ApiResponse<object>>> GetWards([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var wards = await getWardsQueryHandler.Handle(new GetWardsQuery(tenantCode), cancellationToken);
        return Ok(ApiResponse<object>.Ok(wards));
    }

    [HttpGet("beds")]
    [HttpGet("/api/operations/beds")]
    public async Task<ActionResult<ApiResponse<object>>> GetBeds([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var beds = await getBedsQueryHandler.Handle(new GetBedsQuery(tenantCode), cancellationToken);
        return Ok(ApiResponse<object>.Ok(beds));
    }

    [HttpPost("allocate-bed")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> AllocateBed(
        [FromBody] AllocateBedCommand command,
        CancellationToken cancellationToken)
    {
        var result = await allocateBedCommandHandler.Handle(command, cancellationToken);
        return Ok(ApiResponse<CommandResult>.Ok(result, "Bed allocated successfully."));
    }

    [HttpPatch("beds/{id:guid}/status")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> UpdateBedStatus(
        Guid id,
        [FromBody] UpdateBedStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await updateBedStatusCommandHandler.Handle(new UpdateBedStatusCommand(id, request.Status), cancellationToken);
        return Ok(ApiResponse<CommandResult>.Ok(result, "Bed status updated successfully."));
    }
}

public sealed record UpdateBedStatusRequest(string Status);



