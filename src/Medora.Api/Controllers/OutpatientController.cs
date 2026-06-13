using Medora.Application.Common.Models;
using Medora.Application.Features.Outpatient.Commands.BookOutpatientVisit;
using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/outpatient")]
public sealed class OutpatientController(
    IOperationsDataQueryService operationsData,
    IBookOutpatientVisitCommandHandler bookOutpatientVisitCommandHandler) : ControllerBase
{
    [HttpGet("visits")]
    [HttpGet("/api/operations/outpatient-visits")]
    public async Task<ActionResult<ApiResponse<object>>> GetVisits(
        [FromQuery] string? tenantCode,
        CancellationToken cancellationToken)
    {
        var visits = await operationsData.GetEncountersAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(visits));
    }

    [HttpPost("visits")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> BookVisit(
        [FromBody] BookOutpatientVisitCommand command,
        CancellationToken cancellationToken)
    {
        var result = await bookOutpatientVisitCommandHandler.Handle(command, cancellationToken);
        return Created($"/api/outpatient/visits/{result.Id}", ApiResponse<CommandResult>.Ok(result, "Outpatient visit booked successfully."));
    }
}
