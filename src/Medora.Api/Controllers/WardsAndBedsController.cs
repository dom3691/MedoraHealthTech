using Medora.Application.Common.Models;
using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/wards-and-beds")]
public sealed class WardsAndBedsController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet("wards")]
    [HttpGet("/api/operations/wards")]
    public async Task<ActionResult<ApiResponse<object>>> GetWards([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var wards = await operationsData.GetWardsAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(wards));
    }

    [HttpGet("beds")]
    [HttpGet("/api/operations/beds")]
    public async Task<ActionResult<ApiResponse<object>>> GetBeds([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var beds = await operationsData.GetBedsAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(beds));
    }
}
