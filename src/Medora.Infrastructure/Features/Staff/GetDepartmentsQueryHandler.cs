using Medora.Application.Features.Staff.Queries.GetDepartments;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;

namespace Medora.Infrastructure.Features.Staff;

public sealed class GetDepartmentsQueryHandler(MedoraDbContext dbContext) : TenantReadQueryHandlerBase(dbContext), IGetDepartmentsQueryHandler
{
    public Task<object> Handle(GetDepartmentsQuery query, CancellationToken cancellationToken = default) =>
        TenantSetAsync(query.TenantCode, DbContext.Departments.OrderBy(x => x.Name), cancellationToken);
}
