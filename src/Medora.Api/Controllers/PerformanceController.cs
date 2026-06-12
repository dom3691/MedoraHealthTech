using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/hr/performance-reviews")]
public sealed class PerformanceController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetReviews([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var reviews = await operationsData.GetReviewsAsync(tenantCode, cancellationToken);
        return Ok(reviews);
    }
}
