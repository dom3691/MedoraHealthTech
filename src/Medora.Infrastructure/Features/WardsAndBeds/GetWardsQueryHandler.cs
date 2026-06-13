using Medora.Application.Features.WardsAndBeds.Queries.GetWards;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;

namespace Medora.Infrastructure.Features.WardsAndBeds;

public sealed class GetWardsQueryHandler(MedoraDbContext dbContext) : TenantReadQueryHandlerBase(dbContext), IGetWardsQueryHandler
{
    public Task<object> Handle(GetWardsQuery query, CancellationToken cancellationToken = default) =>
        TenantSetAsync(query.TenantCode, DbContext.Wards.OrderBy(x => x.Name), cancellationToken);
}
