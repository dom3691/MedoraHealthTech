using Medora.Application.Common.Models;
using Medora.Application.Features.RadiologyImaging.Commands.CreateRadiologyStudy;
using Medora.Application.Features.RadiologyImaging.Commands.UpdateRadiologyStudyStatus;
using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/radiology-imaging")]
public sealed class RadiologyImagingController(
    IOperationsDataQueryService operationsData,
    ICreateRadiologyStudyCommandHandler createRadiologyStudyCommandHandler,
    IUpdateRadiologyStudyStatusCommandHandler updateRadiologyStudyStatusCommandHandler) : ControllerBase
{
    [HttpGet("studies")]
    public async Task<ActionResult<ApiResponse<object>>> GetStudies([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var studies = await operationsData.GetDiagnosticOrdersAsync(tenantCode, "Radiology", cancellationToken);
        return Ok(ApiResponse<object>.Ok(studies));
    }

    [HttpPost("studies")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> CreateStudy(
        [FromBody] CreateRadiologyStudyCommand command,
        CancellationToken cancellationToken)
    {
        var result = await createRadiologyStudyCommandHandler.Handle(command, cancellationToken);
        return Created($"/api/radiology-imaging/studies/{result.Id}", ApiResponse<CommandResult>.Ok(result, "Radiology study created successfully."));
    }

    [HttpPatch("studies/{id:guid}/status")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> UpdateStudyStatus(
        Guid id,
        [FromBody] UpdateRadiologyStudyStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await updateRadiologyStudyStatusCommandHandler.Handle(new UpdateRadiologyStudyStatusCommand(id, request.Status, request.Finding), cancellationToken);
        return Ok(ApiResponse<CommandResult>.Ok(result, "Radiology study status updated successfully."));
    }
}

public sealed record UpdateRadiologyStudyStatusRequest(string Status, string? Finding);
