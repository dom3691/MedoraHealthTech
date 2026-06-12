using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/hr/credentials")]
public sealed class CredentialingController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetCredentials([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var credentials = await operationsData.GetCredentialsAsync(tenantCode, cancellationToken);
        return Ok(credentials);
    }
}
