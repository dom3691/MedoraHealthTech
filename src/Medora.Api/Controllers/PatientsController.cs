using Medora.Application.Common.Models;
using Medora.Application.Features.Patients.Commands.CreatePatient;
using Medora.Application.Features.Patients.DTOs;
using Medora.Application.Features.Patients.Queries.GetPatients;
using Medora.Application.Modules.Operations.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/patients")]
public sealed class PatientsController(
    IGetPatientsQueryHandler getPatientsQueryHandler,
    ICreatePatientCommandHandler createPatientCommandHandler,
    IOperationsDataQueryService operationsData) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<PatientDto>>>> GetPatients(
        [FromQuery] QueryParameters parameters,
        CancellationToken cancellationToken)
    {
        var patients = await getPatientsQueryHandler.Handle(new GetPatientsQuery(parameters), cancellationToken);
        return Ok(ApiResponse<PagedResult<PatientDto>>.Ok(patients));
    }

    [HttpGet("/api/operations/patients")]
    public async Task<ActionResult> GetLegacyPatients(
        [FromQuery] string? tenantCode,
        CancellationToken cancellationToken)
    {
        var patients = await operationsData.GetPatientsAsync(tenantCode, cancellationToken);
        return Ok(patients);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CommandResult>>> CreatePatient(
        [FromBody] CreatePatientCommand command,
        CancellationToken cancellationToken)
    {
        var result = await createPatientCommandHandler.Handle(command, cancellationToken);
        return Created($"/api/patients/{result.Id}", ApiResponse<CommandResult>.Ok(result, "Patient created successfully."));
    }
}
