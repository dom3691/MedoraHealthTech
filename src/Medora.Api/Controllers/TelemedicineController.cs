using Medora.Application.Modules.Operations.Data;
using Medora.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/telemedicine")]
public sealed class TelemedicineController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet("sessions")]
    [HttpGet("/api/operations/telemedicine-sessions")]
    public async Task<ActionResult<ApiResponse<object>>> GetTelemedicineSessions([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var sessions = await operationsData.GetTelemedicineSessionsAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(sessions));
    }
}
