using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/hr/attendance")]
public sealed class AttendanceController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetAttendance([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var attendance = await operationsData.GetAttendanceAsync(tenantCode, cancellationToken);
        return Ok(attendance);
    }
}
