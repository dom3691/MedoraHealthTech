using Medora.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/hr/credentials")]
public sealed class CredentialingController(ITenantReadModelService tenantReadModels) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetCredentials([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var credentials = await tenantReadModels.GetCredentialsAsync(tenantCode, cancellationToken);
        return Ok(credentials);
    }
}


