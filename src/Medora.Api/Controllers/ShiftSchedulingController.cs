using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/hr/shifts")]
public sealed class ShiftSchedulingController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetShifts([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var shifts = await operationsData.GetShiftsAsync(tenantCode, cancellationToken);
        return Ok(shifts);
    }
}
