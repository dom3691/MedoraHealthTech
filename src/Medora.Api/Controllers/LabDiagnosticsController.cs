using Medora.Application.Common.Models;
using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/lab-diagnostics")]
public sealed class LabDiagnosticsController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet("orders")]
    [HttpGet("/api/operations/diagnostic-orders")]
    public async Task<ActionResult<ApiResponse<object>>> GetOrders(
        [FromQuery] string? tenantCode,
        [FromQuery] string? category,
        CancellationToken cancellationToken)
    {
        var orders = await operationsData.GetDiagnosticOrdersAsync(tenantCode, category, cancellationToken);
        return Ok(ApiResponse<object>.Ok(orders));
    }
}
