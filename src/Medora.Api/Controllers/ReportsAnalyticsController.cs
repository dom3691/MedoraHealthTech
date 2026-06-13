using Medora.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/report-metrics")]
public sealed class ReportsAnalyticsController(ITenantReadModelService tenantReadModels) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetReportMetrics([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var metrics = await tenantReadModels.GetReportMetricsAsync(tenantCode, cancellationToken);
        return Ok(metrics);
    }
}


