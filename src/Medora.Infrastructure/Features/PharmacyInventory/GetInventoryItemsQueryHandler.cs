using Medora.Application.Features.PharmacyInventory.Queries.GetInventoryItems;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;

namespace Medora.Infrastructure.Features.PharmacyInventory;

public sealed class GetInventoryItemsQueryHandler(MedoraDbContext dbContext) : TenantReadQueryHandlerBase(dbContext), IGetInventoryItemsQueryHandler
{
    public Task<object> Handle(GetInventoryItemsQuery query, CancellationToken cancellationToken = default) =>
        TenantSetAsync(query.TenantCode, DbContext.InventoryItems.OrderBy(x => x.Name), cancellationToken);
}
