using Medora.Application.Features.PharmacyInventory.Queries.GetInventoryItems;
using Medora.Application.Common.Models;
using Medora.Application.Features.PharmacyInventory.Commands.AddInventoryItem;
using Medora.Application.Features.PharmacyInventory.Commands.ReorderStock;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/pharmacy-inventory")]
public sealed class PharmacyInventoryController(
    IGetInventoryItemsQueryHandler getInventoryItemsQueryHandler,
    IAddInventoryItemCommandHandler addInventoryItemCommandHandler,
    IReorderStockCommandHandler reorderStockCommandHandler) : ControllerBase
{
    [HttpGet("items")]
    [HttpGet("/api/operations/inventory-items")]
    public async Task<ActionResult<ApiResponse<object>>> GetInventoryItems([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var items = await getInventoryItemsQueryHandler.Handle(new GetInventoryItemsQuery(tenantCode), cancellationToken);
        return Ok(ApiResponse<object>.Ok(items));
    }

    [HttpPost("items")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> AddItem(
        [FromBody] AddInventoryItemCommand command,
        CancellationToken cancellationToken)
    {
        var result = await addInventoryItemCommandHandler.Handle(command, cancellationToken);
        return Created($"/api/pharmacy-inventory/items/{result.Id}", ApiResponse<CommandResult>.Ok(result, "Inventory item added successfully."));
    }

    [HttpPost("reorders")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> ReorderStock(
        [FromBody] ReorderStockCommand command,
        CancellationToken cancellationToken)
    {
        var result = await reorderStockCommandHandler.Handle(command, cancellationToken);
        return Created($"/api/procurement-vendors/purchase-orders/{result.Id}", ApiResponse<CommandResult>.Ok(result, "Reorder purchase order created successfully."));
    }
}




