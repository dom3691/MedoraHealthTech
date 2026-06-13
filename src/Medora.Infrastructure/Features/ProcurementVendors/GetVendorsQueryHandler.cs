using Medora.Application.Features.ProcurementVendors.Queries.GetVendors;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;

namespace Medora.Infrastructure.Features.ProcurementVendors;

public sealed class GetVendorsQueryHandler(MedoraDbContext dbContext) : TenantReadQueryHandlerBase(dbContext), IGetVendorsQueryHandler
{
    public Task<object> Handle(GetVendorsQuery query, CancellationToken cancellationToken = default) =>
        TenantSetAsync(query.TenantCode, DbContext.Vendors.OrderBy(x => x.Name), cancellationToken);
}
