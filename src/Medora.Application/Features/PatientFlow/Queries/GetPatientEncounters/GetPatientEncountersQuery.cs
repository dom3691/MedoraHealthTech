using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.PatientFlow.Queries.GetPatientEncounters;

public sealed record GetPatientEncountersQuery(string? TenantCode) : IQuery<object>;

public interface IGetPatientEncountersQueryHandler : IQueryHandler<GetPatientEncountersQuery, object>;
