using Medora.Application.Features.TheatreAndSurgery.Queries.GetSurgeryCases;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;

namespace Medora.Infrastructure.Features.TheatreAndSurgery;

public sealed class GetSurgeryCasesQueryHandler(MedoraDbContext dbContext) : TenantReadQueryHandlerBase(dbContext), IGetSurgeryCasesQueryHandler
{
    public Task<object> Handle(GetSurgeryCasesQuery query, CancellationToken cancellationToken = default) =>
        TenantSetAsync(query.TenantCode, DbContext.SurgeryCases.OrderBy(x => x.ScheduledAt), cancellationToken);
}
