using Medora.Application.Features.Emergency.Queries.GetEmergencyCases;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;

namespace Medora.Infrastructure.Features.Emergency;

public sealed class GetEmergencyCasesQueryHandler(MedoraDbContext dbContext) : TenantReadQueryHandlerBase(dbContext), IGetEmergencyCasesQueryHandler
{
    public Task<object> Handle(GetEmergencyCasesQuery query, CancellationToken cancellationToken = default) =>
        TenantSetAsync(query.TenantCode, DbContext.EmergencyCases.OrderBy(x => x.ArrivedAt), cancellationToken);
}
