using Medora.Application.Features.Nursing.Queries.GetNursingTasks;
using Medora.Application.Common.Models;
using Medora.Application.Features.Nursing.Commands.CreateNursingTask;
using Medora.Application.Features.Nursing.Commands.UpdateNursingTaskStatus;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/nursing")]
public sealed class NursingController(
    IGetNursingTasksQueryHandler getNursingTasksQueryHandler,
    ICreateNursingTaskCommandHandler createNursingTaskCommandHandler,
    IUpdateNursingTaskStatusCommandHandler updateNursingTaskStatusCommandHandler) : ControllerBase
{
    [HttpGet("tasks")]
    [HttpGet("/api/operations/nursing-tasks")]
    public async Task<ActionResult<ApiResponse<object>>> GetNursingTasks([FromQuery] string? tenantCode, CancellationToken cancellationToken)
    {
        var tasks = await getNursingTasksQueryHandler.Handle(new GetNursingTasksQuery(tenantCode), cancellationToken);
        return Ok(ApiResponse<object>.Ok(tasks));
    }

    [HttpPost("tasks")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> CreateTask(
        [FromBody] CreateNursingTaskCommand command,
        CancellationToken cancellationToken)
    {
        var result = await createNursingTaskCommandHandler.Handle(command, cancellationToken);
        return Created($"/api/nursing/tasks/{result.Id}", ApiResponse<CommandResult>.Ok(result, "Nursing task created successfully."));
    }

    [HttpPatch("tasks/{id:guid}/status")]
    public async Task<ActionResult<ApiResponse<CommandResult>>> UpdateTaskStatus(
        Guid id,
        [FromBody] UpdateNursingTaskStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await updateNursingTaskStatusCommandHandler.Handle(new UpdateNursingTaskStatusCommand(id, request.Status), cancellationToken);
        return Ok(ApiResponse<CommandResult>.Ok(result, "Nursing task status updated successfully."));
    }
}

public sealed record UpdateNursingTaskStatusRequest(string Status);



