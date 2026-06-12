using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;
using Medora.Application.Features.Patients.DTOs;

namespace Medora.Application.Features.Patients.Queries.GetPatients;

public sealed record GetPatientsQuery(QueryParameters Parameters) : IQuery<PagedResult<PatientDto>>;
