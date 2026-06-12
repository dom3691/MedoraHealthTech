using Medora.Application.Modules.Operations.Data;
using Medora.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/pharmacy-inventory")]
public sealed class PharmacyInventoryController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet("items")]
    [HttpGet("/api/operations/inventory-items")]
    public async Task<ActionResult<ApiResponse<object>>> GetInventoryItems([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var items = await operationsData.GetInventoryItemsAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(items));
    }
}
