using Medora.Application.Features.Telemedicine.Queries.GetTelemedicineSessions;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;

namespace Medora.Infrastructure.Features.Telemedicine;

public sealed class GetTelemedicineSessionsQueryHandler(MedoraDbContext dbContext) : TenantReadQueryHandlerBase(dbContext), IGetTelemedicineSessionsQueryHandler
{
    public Task<object> Handle(GetTelemedicineSessionsQuery query, CancellationToken cancellationToken = default) =>
        TenantSetAsync(query.TenantCode, DbContext.TelemedicineSessions.OrderBy(x => x.ScheduledAt), cancellationToken);
}
