using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/modules")]
public sealed class OperationalModulesController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetModules([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var modules = await operationsData.GetModulesAsync(tenantCode, cancellationToken);
        return Ok(modules);
    }
}
