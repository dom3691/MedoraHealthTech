using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations")]
public sealed class AuditComplianceController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet("audit-logs")]
    public async Task<ActionResult> GetAuditLogs([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var auditLogs = await operationsData.GetAuditLogsAsync(tenantCode, cancellationToken);
        return Ok(auditLogs);
    }

    [HttpGet("compliance-controls")]
    public async Task<ActionResult> GetComplianceControls([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var controls = await operationsData.GetComplianceControlsAsync(tenantCode, cancellationToken);
        return Ok(controls);
    }
}
