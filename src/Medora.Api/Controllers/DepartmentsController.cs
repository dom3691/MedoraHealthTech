using Medora.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/departments")]
public sealed class DepartmentsController(ITenantReadModelService tenantReadModels) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetDepartments([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var departments = await tenantReadModels.GetDepartmentsAsync(tenantCode, cancellationToken);
        return Ok(departments);
    }
}


