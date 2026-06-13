using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.Tenants.Queries.GetTenants;

public sealed record GetTenantsQuery() : IQuery<object>;

public interface IGetTenantsQueryHandler : IQueryHandler<GetTenantsQuery, object>;

