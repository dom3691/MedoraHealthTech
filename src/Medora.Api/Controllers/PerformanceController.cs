using Medora.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/hr/performance-reviews")]
public sealed class PerformanceController(ITenantReadModelService tenantReadModels) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetReviews([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var reviews = await tenantReadModels.GetReviewsAsync(tenantCode, cancellationToken);
        return Ok(reviews);
    }
}


