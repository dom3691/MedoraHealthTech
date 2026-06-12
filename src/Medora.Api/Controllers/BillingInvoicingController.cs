using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/invoices")]
public sealed class BillingInvoicingController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetInvoices([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var invoices = await operationsData.GetInvoicesAsync(tenantCode, cancellationToken);
        return Ok(invoices);
    }
}
