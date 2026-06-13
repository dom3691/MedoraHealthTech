using Medora.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/invoices")]
public sealed class BillingInvoicingController(ITenantReadModelService tenantReadModels) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetInvoices([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var invoices = await tenantReadModels.GetInvoicesAsync(tenantCode, cancellationToken);
        return Ok(invoices);
    }
}


