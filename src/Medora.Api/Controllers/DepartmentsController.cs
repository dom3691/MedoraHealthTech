using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/operations/departments")]
public sealed class DepartmentsController(IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetDepartments([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var departments = await operationsData.GetDepartmentsAsync(tenantCode, cancellationToken);
        return Ok(departments);
    }
}
