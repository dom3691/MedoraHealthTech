using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.Notifications.Commands.CreateNotification;

public sealed record CreateNotificationCommand(
    string Category,
    string Title,
    string Body,
    string Severity) : ICommand<CommandResult>;

public interface ICreateNotificationCommandHandler : ICommandHandler<CreateNotificationCommand, CommandResult>;
