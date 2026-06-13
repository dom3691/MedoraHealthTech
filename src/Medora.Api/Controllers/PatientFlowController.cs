using Medora.Application.Features.PatientFlow.Queries.GetPatientEncounters;
using Medora.Application.Common.Models;
using Medora.Application.Features.PatientFlow.Commands.MovePatientFlowStage;
using Medora.Application.Features.PatientFlow.Commands.RegisterWalkIn;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/patient-flow")]
public sealed class PatientFlowController(
    IGetPatientEncountersQueryHandler getPatientEncountersQueryHandler,
    IRegisterWalkInCommandHandler registerWalkInCommandHandler,
    IMovePatientFlowStageCommandHandler movePatientFlowStageCommandHandler) : ControllerBase
{
    [HttpGet]
    [HttpGet("encounters")]
    [HttpGet("/api/operations/encounters")]
    public async Task<ActionResult<ApiResponse<object>>> GetEncounters([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var encounters = await getPatientEncountersQueryHandler.Handle(new GetPatientEncountersQuery(tenantCode), cancellationToken);
        return Ok(ApiResponse<object>.Ok(encounters));
    }

    [HttpPost("walk-ins")]
    [HttpPost("/api/operations/encounters")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> RegisterWalkIn(
        [FromBody] RegisterWalkInCommand command,
        CancellationToken cancellationToken)
    {
        var result = await registerWalkInCommandHandler.Handle(command, cancellationToken);
        return Created($"/api/patient-flow/encounters/{result.Id}", ApiResponse<CommandResult>.Ok(result, "Walk-in registered successfully."));
    }

    [HttpPatch("encounters/{id:guid}/stage")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> MoveStage(
        Guid id,
        [FromBody] MovePatientFlowStageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await movePatientFlowStageCommandHandler.Handle(new MovePatientFlowStageCommand(id, request.Stage, request.Status), cancellationToken);
        return Ok(ApiResponse<CommandResult>.Ok(result, "Patient flow stage updated successfully."));
    }
}

public sealed record MovePatientFlowStageRequest(string Stage, string Status);



