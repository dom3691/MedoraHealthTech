using Medora.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations")]
public sealed class AuditComplianceController(ITenantReadModelService tenantReadModels) : ControllerBase
{
    [HttpGet("audit-logs")]
    public async Task<ActionResult> GetAuditLogs([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var auditLogs = await tenantReadModels.GetAuditLogsAsync(tenantCode, cancellationToken);
        return Ok(auditLogs);
    }

    [HttpGet("compliance-controls")]
    public async Task<ActionResult> GetComplianceControls([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var controls = await tenantReadModels.GetComplianceControlsAsync(tenantCode, cancellationToken);
        return Ok(controls);
    }
}


