using Medora.Application.Features.Notifications.Queries.GetNotifications;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;

namespace Medora.Infrastructure.Features.Notifications;

public sealed class GetNotificationsQueryHandler(MedoraDbContext dbContext) : TenantReadQueryHandlerBase(dbContext), IGetNotificationsQueryHandler
{
    public Task<object> Handle(GetNotificationsQuery query, CancellationToken cancellationToken = default) =>
        TenantSetAsync(query.TenantCode, DbContext.Notifications.OrderByDescending(x => x.PublishedAt), cancellationToken);
}
