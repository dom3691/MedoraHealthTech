using Medora.Application.Common.Models;
using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/theatre-and-surgery")]
public sealed class TheatreAndSurgeryController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet("cases")]
    [HttpGet("/api/operations/surgery-cases")]
    public async Task<ActionResult<ApiResponse<object>>> GetSurgeryCases([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var cases = await operationsData.GetSurgeryCasesAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(cases));
    }
}
