using Medora.Application.Features.WardsAndBeds.Queries.GetBeds;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;

namespace Medora.Infrastructure.Features.WardsAndBeds;

public sealed class GetBedsQueryHandler(MedoraDbContext dbContext) : TenantReadQueryHandlerBase(dbContext), IGetBedsQueryHandler
{
    public Task<object> Handle(GetBedsQuery query, CancellationToken cancellationToken = default) =>
        TenantSetAsync(query.TenantCode, DbContext.Beds.OrderBy(x => x.BedNumber), cancellationToken);
}
