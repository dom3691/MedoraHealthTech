using Medora.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/hr/shifts")]
public sealed class ShiftSchedulingController(ITenantReadModelService tenantReadModels) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetShifts([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var shifts = await tenantReadModels.GetShiftsAsync(tenantCode, cancellationToken);
        return Ok(shifts);
    }
}


