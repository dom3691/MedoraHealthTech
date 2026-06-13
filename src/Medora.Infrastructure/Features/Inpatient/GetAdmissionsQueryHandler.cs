using Medora.Application.Features.Inpatient.Queries.GetAdmissions;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;

namespace Medora.Infrastructure.Features.Inpatient;

public sealed class GetAdmissionsQueryHandler(MedoraDbContext dbContext) : TenantReadQueryHandlerBase(dbContext), IGetAdmissionsQueryHandler
{
    public Task<object> Handle(GetAdmissionsQuery query, CancellationToken cancellationToken = default) =>
        TenantSetAsync(query.TenantCode, DbContext.Admissions.OrderByDescending(x => x.AdmittedAt), cancellationToken);
}
