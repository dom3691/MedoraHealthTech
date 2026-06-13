using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.Emergency.Queries.GetEmergencyCases;

public sealed record GetEmergencyCasesQuery(string? TenantCode) : IQuery<object>;

public interface IGetEmergencyCasesQueryHandler : IQueryHandler<GetEmergencyCasesQuery, object>;
