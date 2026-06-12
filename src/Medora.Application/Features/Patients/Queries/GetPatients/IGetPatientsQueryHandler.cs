using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;
using Medora.Application.Features.Patients.DTOs;

namespace Medora.Application.Features.Patients.Queries.GetPatients;

public interface IGetPatientsQueryHandler : IQueryHandler<GetPatientsQuery, PagedResult<PatientDto>>
{
}
