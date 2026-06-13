using Medora.Application.Modules.Operations.Data;
using Medora.Application.Common.Models;
using Medora.Application.Features.Staff.Commands.AddStaffMember;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/staff")]
public sealed class StaffController(
    IOperationsDataQueryService operationsData,
    IAddStaffMemberCommandHandler addStaffMemberCommandHandler) : ControllerBase
{
    [HttpGet]
    [HttpGet("/api/operations/staff")]
    public async Task<ActionResult<ApiResponse<object>>> GetStaff([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var staff = await operationsData.GetStaffAsync(tenantCode, cancellationToken);
        return Ok(ApiResponse<object>.Ok(staff));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CommandResult>>> AddStaffMember(
        [FromBody] AddStaffMemberCommand command,
        CancellationToken cancellationToken)
    {
        var result = await addStaffMemberCommandHandler.Handle(command, cancellationToken);
        return Created($"/api/staff/{result.Id}", ApiResponse<CommandResult>.Ok(result, "Staff member added successfully."));
    }
}
