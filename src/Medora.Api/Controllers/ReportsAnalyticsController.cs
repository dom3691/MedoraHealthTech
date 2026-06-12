using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/report-metrics")]
public sealed class ReportsAnalyticsController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetReportMetrics([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var metrics = await operationsData.GetReportMetricsAsync(tenantCode, cancellationToken);
        return Ok(metrics);
    }
}
