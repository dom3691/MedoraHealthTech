using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.Outpatient.Queries.GetOutpatientVisits;

public sealed record GetOutpatientVisitsQuery(string? TenantCode) : IQuery<object>;

public interface IGetOutpatientVisitsQueryHandler : IQueryHandler<GetOutpatientVisitsQuery, object>;
