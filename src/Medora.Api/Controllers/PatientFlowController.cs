using Medora.Application.Modules.Operations.Data;
using Medora.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/patient-flow")]
public sealed class PatientFlowController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet]
    [HttpGet("encounters")]
    [HttpGet("/api/operations/encounters")]
    public async Task<ActionResult<ApiResponse<object>>> GetEncounters([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var encounters = await operationsData.GetEncountersAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(encounters));
    }
}
