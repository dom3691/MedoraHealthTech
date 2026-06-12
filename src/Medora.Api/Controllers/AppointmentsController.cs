using Medora.Application.Common.Models;
using Medora.Application.Features.Appointments.DTOs;
using Medora.Application.Features.Appointments.Queries.GetAppointments;
using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/appointments")]
public sealed class AppointmentsController(
    IGetAppointmentsQueryHandler getAppointmentsQueryHandler,
    IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<AppointmentDto>>>> GetAppointments(
        [FromQuery] QueryParameters parameters,
        [FromQuery] string? department,
        [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        var appointments = await getAppointmentsQueryHandler.Handle(
            new GetAppointmentsQuery(parameters, department, status),
            cancellationToken);

        return Ok(ApiResponse<PagedResult<AppointmentDto>>.Ok(appointments));
    }

    [HttpGet("/api/operations/appointments")]
    public async Task<ActionResult> GetLegacyAppointments(
        [FromQuery] string? tenantCode,
        CancellationToken cancellationToken)
    {
        var appointments = await operationsData.GetAppointmentsAsync(tenantCode, cancellationToken);
        return Ok(appointments);
    }
}
