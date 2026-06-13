using Medora.Application.Features.Staff.Queries.GetStaff;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;

namespace Medora.Infrastructure.Features.Staff;

public sealed class GetStaffQueryHandler(MedoraDbContext dbContext) : TenantReadQueryHandlerBase(dbContext), IGetStaffQueryHandler
{
    public Task<object> Handle(GetStaffQuery query, CancellationToken cancellationToken = default) =>
        TenantSetAsync(query.TenantCode, DbContext.StaffMembers.OrderBy(x => x.FullName), cancellationToken);
}
