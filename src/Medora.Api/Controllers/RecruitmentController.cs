using Medora.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/hr/candidates")]
public sealed class RecruitmentController(ITenantReadModelService tenantReadModels) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetCandidates([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var candidates = await tenantReadModels.GetCandidatesAsync(tenantCode, cancellationToken);
        return Ok(candidates);
    }
}


