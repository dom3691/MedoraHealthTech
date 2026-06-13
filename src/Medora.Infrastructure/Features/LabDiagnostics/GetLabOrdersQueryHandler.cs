using Medora.Application.Features.LabDiagnostics.Queries.GetLabOrders;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Medora.Infrastructure.Features.LabDiagnostics;

public sealed class GetLabOrdersQueryHandler(MedoraDbContext dbContext) : TenantReadQueryHandlerBase(dbContext), IGetLabOrdersQueryHandler
{
    public Task<object> Handle(GetLabOrdersQuery query, CancellationToken cancellationToken = default) =>
        TenantSetAsync(query.TenantCode, DbContext.DiagnosticOrders.Where(x => x.Category == "Lab").OrderBy(x => x.OrderedAt), cancellationToken);
}
