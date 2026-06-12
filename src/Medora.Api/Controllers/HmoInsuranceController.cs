using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations")]
public sealed class HmoInsuranceController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet("insurance-claims")]
    public async Task<ActionResult> GetInsuranceClaims([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var claims = await operationsData.GetInsuranceClaimsAsync(tenantCode, cancellationToken);
        return Ok(claims);
    }

    [HttpGet("pre-authorizations")]
    public async Task<ActionResult> GetPreAuthorizations([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var preAuthorizations = await operationsData.GetPreAuthorizationsAsync(tenantCode, cancellationToken);
        return Ok(preAuthorizations);
    }
}
