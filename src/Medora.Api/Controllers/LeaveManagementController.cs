using Medora.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/hr/leave-requests")]
public sealed class LeaveManagementController(ITenantReadModelService tenantReadModels) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetLeaveRequests([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var leaveRequests = await tenantReadModels.GetLeaveRequestsAsync(tenantCode, cancellationToken);
        return Ok(leaveRequests);
    }
}


