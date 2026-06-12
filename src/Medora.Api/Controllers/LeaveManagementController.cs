using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/hr/leave-requests")]
public sealed class LeaveManagementController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetLeaveRequests([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var leaveRequests = await operationsData.GetLeaveRequestsAsync(tenantCode, cancellationToken);
        return Ok(leaveRequests);
    }
}
