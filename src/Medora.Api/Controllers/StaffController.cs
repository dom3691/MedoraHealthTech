using Medora.Application.Modules.Operations.Data;
using Medora.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/staff")]
public sealed class StaffController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet]
    [HttpGet("/api/operations/staff")]
    public async Task<ActionResult<ApiResponse<object>>> GetStaff([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var staff = await operationsData.GetStaffAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(staff));
    }
}
