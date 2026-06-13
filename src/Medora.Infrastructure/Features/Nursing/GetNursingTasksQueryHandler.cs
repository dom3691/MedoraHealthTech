using Medora.Application.Features.Nursing.Queries.GetNursingTasks;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;

namespace Medora.Infrastructure.Features.Nursing;

public sealed class GetNursingTasksQueryHandler(MedoraDbContext dbContext) : TenantReadQueryHandlerBase(dbContext), IGetNursingTasksQueryHandler
{
    public Task<object> Handle(GetNursingTasksQuery query, CancellationToken cancellationToken = default) =>
        TenantSetAsync(query.TenantCode, DbContext.NursingTasks.OrderBy(x => x.DueAt), cancellationToken);
}
