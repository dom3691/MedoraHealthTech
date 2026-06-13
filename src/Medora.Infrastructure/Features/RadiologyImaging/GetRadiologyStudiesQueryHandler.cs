using Medora.Application.Features.RadiologyImaging.Queries.GetRadiologyStudies;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Medora.Infrastructure.Features.RadiologyImaging;

public sealed class GetRadiologyStudiesQueryHandler(MedoraDbContext dbContext) : TenantReadQueryHandlerBase(dbContext), IGetRadiologyStudiesQueryHandler
{
    public Task<object> Handle(GetRadiologyStudiesQuery query, CancellationToken cancellationToken = default) =>
        TenantSetAsync(query.TenantCode, DbContext.DiagnosticOrders.Where(x => x.Category == "Radiology").OrderBy(x => x.OrderedAt), cancellationToken);
}
