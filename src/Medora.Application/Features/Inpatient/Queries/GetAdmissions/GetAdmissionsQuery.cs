using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.Inpatient.Queries.GetAdmissions;

public sealed record GetAdmissionsQuery(string? TenantCode) : IQuery<object>;

public interface IGetAdmissionsQueryHandler : IQueryHandler<GetAdmissionsQuery, object>;
