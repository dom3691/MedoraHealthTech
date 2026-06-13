using Medora.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/hr/attendance")]
public sealed class AttendanceController(ITenantReadModelService tenantReadModels) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetAttendance([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var attendance = await tenantReadModels.GetAttendanceAsync(tenantCode, cancellationToken);
        return Ok(attendance);
    }
}


