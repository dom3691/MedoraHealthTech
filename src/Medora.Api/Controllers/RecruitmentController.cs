using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/hr/candidates")]
public sealed class RecruitmentController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetCandidates([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var candidates = await operationsData.GetCandidatesAsync(tenantCode, cancellationToken);
        return Ok(candidates);
    }
}
