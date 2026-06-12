using Medora.Application.Modules.Operations.Data;
using Medora.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/notifications")]
public sealed class NotificationsController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet]
    [HttpGet("/api/operations/notifications")]
    public async Task<ActionResult<ApiResponse<object>>> GetNotifications([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var notifications = await operationsData.GetNotificationsAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(notifications));
    }
}
