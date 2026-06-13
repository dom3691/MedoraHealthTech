using Medora.Application.Common.Models;
using Medora.Application.Features.LabDiagnostics.Commands.CreateLabOrder;
using Medora.Application.Features.LabDiagnostics.Commands.UpdateLabOrderStatus;
using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/lab-diagnostics")]
public sealed class LabDiagnosticsController(
    IOperationsDataQueryService operationsData,
    ICreateLabOrderCommandHandler createLabOrderCommandHandler,
    IUpdateLabOrderStatusCommandHandler updateLabOrderStatusCommandHandler) : ControllerBase
{
    [HttpGet("orders")]
    [HttpGet("/api/operations/diagnostic-orders")]
    public async Task<ActionResult<ApiResponse<object>>> GetOrders(
        [FromQuery] string? tenantCode,
        [FromQuery] string? category,
        CancellationToken cancellationToken)
    {
        var orders = await operationsData.GetDiagnosticOrdersAsync(tenantCode, category, cancellationToken);
        return Ok(ApiResponse<object>.Ok(orders));
    }

    [HttpPost("orders")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> CreateOrder(
        [FromBody] CreateLabOrderCommand command,
        CancellationToken cancellationToken)
    {
        var result = await createLabOrderCommandHandler.Handle(command, cancellationToken);
        return Created($"/api/lab-diagnostics/orders/{result.Id}", ApiResponse<CommandResult>.Ok(result, "Lab order created successfully."));
    }

    [HttpPatch("orders/{id:guid}/status")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> UpdateOrderStatus(
        Guid id,
        [FromBody] UpdateLabOrderStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await updateLabOrderStatusCommandHandler.Handle(new UpdateLabOrderStatusCommand(id, request.Status, request.Finding), cancellationToken);
        return Ok(ApiResponse<CommandResult>.Ok(result, "Lab order status updated successfully."));
    }
}

public sealed record UpdateLabOrderStatusRequest(string Status, string? Finding);
