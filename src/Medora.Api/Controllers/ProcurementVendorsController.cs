using Medora.Application.Modules.Operations.Data;
using Medora.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/procurement-vendors")]
public sealed class ProcurementVendorsController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet("vendors")]
    [HttpGet("/api/operations/vendors")]
    public async Task<ActionResult<ApiResponse<object>>> GetVendors([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var vendors = await operationsData.GetVendorsAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(vendors));
    }

    [HttpGet("purchase-orders")]
    [HttpGet("/api/operations/purchase-orders")]
    public async Task<ActionResult<ApiResponse<object>>> GetPurchaseOrders([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var purchaseOrders = await operationsData.GetPurchaseOrdersAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(purchaseOrders));
    }
}
