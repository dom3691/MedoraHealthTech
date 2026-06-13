using Medora.Application.Common.Models;
using Medora.Application.Features.Inpatient.Commands.AdmitPatient;
using Medora.Application.Features.Inpatient.Commands.DischargePatient;
using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/inpatient")]
public sealed class InpatientController(
    IOperationsDataQueryService operationsData,
    IAdmitPatientCommandHandler admitPatientCommandHandler,
    IDischargePatientCommandHandler dischargePatientCommandHandler) : ControllerBase
{
    [HttpGet("admissions")]
    [HttpGet("/api/operations/admissions")]
    public async Task<ActionResult<ApiResponse<object>>> GetAdmissions([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var admissions = await operationsData.GetAdmissionsAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(admissions));
    }

    [HttpPost("admissions")]
    [HttpPost("/api/operations/admissions")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> AdmitPatient(
        [FromBody] AdmitPatientCommand command,
        CancellationToken cancellationToken)
    {
        var result = await admitPatientCommandHandler.Handle(command, cancellationToken);
        return Created($"/api/inpatient/admissions/{result.Id}", ApiResponse<CommandResult>.Ok(result, "Patient admitted successfully."));
    }

    [HttpPatch("admissions/{id:guid}/discharge")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> DischargePatient(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await dischargePatientCommandHandler.Handle(new DischargePatientCommand(id), cancellationToken);
        return Ok(ApiResponse<CommandResult>.Ok(result, "Patient discharged successfully."));
    }
}
