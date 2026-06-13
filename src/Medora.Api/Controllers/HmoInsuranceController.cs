using Medora.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations")]
public sealed class HmoInsuranceController(ITenantReadModelService tenantReadModels) : ControllerBase
{
    [HttpGet("insurance-claims")]
    public async Task<ActionResult> GetInsuranceClaims([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var claims = await tenantReadModels.GetInsuranceClaimsAsync(tenantCode, cancellationToken);
        return Ok(claims);
    }

    [HttpGet("pre-authorizations")]
    public async Task<ActionResult> GetPreAuthorizations([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var preAuthorizations = await tenantReadModels.GetPreAuthorizationsAsync(tenantCode, cancellationToken);
        return Ok(preAuthorizations);
    }
}


