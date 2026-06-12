using Medora.Application.Common.Models;
using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/radiology-imaging")]
public sealed class RadiologyImagingController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet("studies")]
    public async Task<ActionResult<ApiResponse<object>>> GetStudies([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var studies = await operationsData.GetDiagnosticOrdersAsync(tenantCode, "Radiology", cancellationToken);
        return Ok(ApiResponse<object>.Ok(studies));
    }
}
