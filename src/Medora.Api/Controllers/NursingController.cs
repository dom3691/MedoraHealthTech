using Medora.Application.Modules.Operations.Data;
using Medora.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/nursing")]
public sealed class NursingController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet("tasks")]
    [HttpGet("/api/operations/nursing-tasks")]
    public async Task<ActionResult<ApiResponse<object>>> GetNursingTasks([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var tasks = await operationsData.GetNursingTasksAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(tasks));
    }
}
