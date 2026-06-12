using Medora.Application.Common.Models;
using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/inpatient")]
public sealed class InpatientController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet("admissions")]
    [HttpGet("/api/operations/admissions")]
    public async Task<ActionResult<ApiResponse<object>>> GetAdmissions([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var admissions = await operationsData.GetAdmissionsAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(admissions));
    }
}
