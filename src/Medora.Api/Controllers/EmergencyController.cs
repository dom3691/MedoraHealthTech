using Medora.Application.Common.Models;
using Medora.Application.Features.Emergency.Commands.RegisterEmergencyArrival;
using Medora.Application.Features.Emergency.Commands.UpdateEmergencyStatus;
using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/emergency")]
public sealed class EmergencyController(
    IOperationsDataQueryService operationsData,
    IRegisterEmergencyArrivalCommandHandler registerEmergencyArrivalCommandHandler,
    IUpdateEmergencyStatusCommandHandler updateEmergencyStatusCommandHandler) : ControllerBase
{
    [HttpGet("cases")]
    [HttpGet("/api/operations/emergency-cases")]
    public async Task<ActionResult<ApiResponse<object>>> GetEmergencyCases([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var cases = await operationsData.GetEmergencyCasesAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(cases));
    }

    [HttpPost("arrivals")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> RegisterArrival(
        [FromBody] RegisterEmergencyArrivalCommand command,
        CancellationToken cancellationToken)
    {
        var result = await registerEmergencyArrivalCommandHandler.Handle(command, cancellationToken);
        return Created($"/api/emergency/cases/{result.Id}", ApiResponse<CommandResult>.Ok(result, "Emergency arrival registered successfully."));
    }

    [HttpPatch("cases/{id:guid}/status")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> UpdateStatus(
        Guid id,
        [FromBody] UpdateEmergencyStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await updateEmergencyStatusCommandHandler.Handle(new UpdateEmergencyStatusCommand(id, request.Status), cancellationToken);
        return Ok(ApiResponse<CommandResult>.Ok(result, "Emergency status updated successfully."));
    }
}

public sealed record UpdateEmergencyStatusRequest(string Status);
