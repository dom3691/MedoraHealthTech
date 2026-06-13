using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.Notifications.Queries.GetNotifications;

public sealed record GetNotificationsQuery(string? TenantCode) : IQuery<object>;

public interface IGetNotificationsQueryHandler : IQueryHandler<GetNotificationsQuery, object>;
