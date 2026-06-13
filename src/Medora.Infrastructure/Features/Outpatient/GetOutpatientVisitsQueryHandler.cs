using Medora.Application.Features.Outpatient.Queries.GetOutpatientVisits;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;

namespace Medora.Infrastructure.Features.Outpatient;

public sealed class GetOutpatientVisitsQueryHandler(MedoraDbContext dbContext) : TenantReadQueryHandlerBase(dbContext), IGetOutpatientVisitsQueryHandler
{
    public Task<object> Handle(GetOutpatientVisitsQuery query, CancellationToken cancellationToken = default) =>
        TenantSetAsync(query.TenantCode, DbContext.PatientEncounters.OrderBy(x => x.ArrivedAt), cancellationToken);
}
