using Medora.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/modules")]
public sealed class OperationalModulesController(ITenantReadModelService tenantReadModels) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetModules([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var modules = await tenantReadModels.GetModulesAsync(tenantCode, cancellationToken);
        return Ok(modules);
    }
}


