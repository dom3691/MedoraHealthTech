using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;
using Medora.Application.Features.Notifications.Commands.CreateNotification;
using Medora.Domain.HospitalOperations;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;

namespace Medora.Infrastructure.Features.Notifications;

public sealed class CreateNotificationCommandHandler(
    MedoraDbContext dbContext,
    ICurrentTenantService currentTenant,
    ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), ICreateNotificationCommandHandler
{
    public async Task<CommandResult> Handle(CreateNotificationCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var notification = new NotificationMessage
        {
            Category = command.Category.Trim(),
            Title = command.Title.Trim(),
            Body = command.Body.Trim(),
            Severity = command.Severity.Trim(),
            Status = "Unread",
            PublishedAt = DateTimeOffset.UtcNow
        };

        StampCreate(notification, tenantId);
        DbContext.Notifications.Add(notification);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(notification, notification.Title, notification.Status);
    }
}
