using Medora.Application.Features.Notifications.Queries.GetNotifications;
using Medora.Application.Common.Models;
using Medora.Application.Features.Notifications.Commands.CreateNotification;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/notifications")]
public sealed class NotificationsController(
    IGetNotificationsQueryHandler getNotificationsQueryHandler,
    ICreateNotificationCommandHandler createNotificationCommandHandler) : ControllerBase
{
    [HttpGet]
    [HttpGet("/api/operations/notifications")]
    public async Task<ActionResult<ApiResponse<object>>> GetNotifications([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var notifications = await getNotificationsQueryHandler.Handle(new GetNotificationsQuery(tenantCode), cancellationToken);
        return Ok(ApiResponse<object>.Ok(notifications));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CommandResult>>> CreateNotification(
        [FromBody] CreateNotificationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await createNotificationCommandHandler.Handle(command, cancellationToken);
        return Created($"/api/notifications/{result.Id}", ApiResponse<CommandResult>.Ok(result, "Notification created successfully."));
    }
}



