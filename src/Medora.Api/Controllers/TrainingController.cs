using Medora.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/hr/training")]
public sealed class TrainingController(ITenantReadModelService tenantReadModels) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetTraining([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var training = await tenantReadModels.GetTrainingAsync(tenantCode, cancellationToken);
        return Ok(training);
    }
}


