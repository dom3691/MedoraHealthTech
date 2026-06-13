using Medora.Application.Features.PatientFlow.Queries.GetPatientEncounters;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;

namespace Medora.Infrastructure.Features.PatientFlow;

public sealed class GetPatientEncountersQueryHandler(MedoraDbContext dbContext) : TenantReadQueryHandlerBase(dbContext), IGetPatientEncountersQueryHandler
{
    public Task<object> Handle(GetPatientEncountersQuery query, CancellationToken cancellationToken = default) =>
        TenantSetAsync(query.TenantCode, DbContext.PatientEncounters.OrderBy(x => x.ArrivedAt), cancellationToken);
}
