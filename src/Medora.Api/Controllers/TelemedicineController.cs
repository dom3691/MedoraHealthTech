using Medora.Application.Features.Telemedicine.Queries.GetTelemedicineSessions;
using Medora.Application.Common.Models;
using Medora.Application.Features.Telemedicine.Commands.ScheduleTelemedicineSession;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/telemedicine")]
public sealed class TelemedicineController(
    IGetTelemedicineSessionsQueryHandler getTelemedicineSessionsQueryHandler,
    IScheduleTelemedicineSessionCommandHandler scheduleTelemedicineSessionCommandHandler) : ControllerBase
{
    [HttpGet("sessions")]
    [HttpGet("/api/operations/telemedicine-sessions")]
    public async Task<ActionResult<ApiResponse<object>>> GetTelemedicineSessions([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var sessions = await getTelemedicineSessionsQueryHandler.Handle(new GetTelemedicineSessionsQuery(tenantCode), cancellationToken);
        return Ok(ApiResponse<object>.Ok(sessions));
    }

    [HttpPost("sessions")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> ScheduleSession(
        [FromBody] ScheduleTelemedicineSessionCommand command,
        CancellationToken cancellationToken)
    {
        var result = await scheduleTelemedicineSessionCommandHandler.Handle(command, cancellationToken);
        return Created($"/api/telemedicine/sessions/{result.Id}", ApiResponse<CommandResult>.Ok(result, "Telemedicine session scheduled successfully."));
    }
}



