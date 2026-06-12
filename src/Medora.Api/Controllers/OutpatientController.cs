using Medora.Application.Common.Models;
using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/outpatient")]
public sealed class OutpatientController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet("visits")]
    [HttpGet("/api/operations/outpatient-visits")]
    public async Task<ActionResult<ApiResponse<object>>> GetVisits(
        [FromQuery] string? tenantCode,
        CancellationToken cancellationToken)
    {
        var visits = await operationsData.GetEncountersAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(visits));
    }
}
