using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/hr/training")]
public sealed class TrainingController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetTraining([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var training = await operationsData.GetTrainingAsync(tenantCode, cancellationToken);
        return Ok(training);
    }
}
