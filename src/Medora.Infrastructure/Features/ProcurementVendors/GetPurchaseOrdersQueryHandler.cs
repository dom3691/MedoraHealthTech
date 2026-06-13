using Medora.Application.Features.ProcurementVendors.Queries.GetPurchaseOrders;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;

namespace Medora.Infrastructure.Features.ProcurementVendors;

public sealed class GetPurchaseOrdersQueryHandler(MedoraDbContext dbContext) : TenantReadQueryHandlerBase(dbContext), IGetPurchaseOrdersQueryHandler
{
    public Task<object> Handle(GetPurchaseOrdersQuery query, CancellationToken cancellationToken = default) =>
        TenantSetAsync(query.TenantCode, DbContext.PurchaseOrders.OrderByDescending(x => x.CreatedAt), cancellationToken);
}
