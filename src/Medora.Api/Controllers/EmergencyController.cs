using Medora.Application.Common.Models;
using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/emergency")]
public sealed class EmergencyController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet("cases")]
    [HttpGet("/api/operations/emergency-cases")]
    public async Task<ActionResult<ApiResponse<object>>> GetEmergencyCases([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var cases = await operationsData.GetEmergencyCasesAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(cases));
    }
}
