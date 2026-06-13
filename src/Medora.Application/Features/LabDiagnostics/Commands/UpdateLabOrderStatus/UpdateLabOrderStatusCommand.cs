using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.LabDiagnostics.Commands.UpdateLabOrderStatus;

public sealed record UpdateLabOrderStatusCommand(Guid OrderId, string Status, string? Finding) : ICommand<CommandResult>;

public interface IUpdateLabOrderStatusCommandHandler : ICommandHandler<UpdateLabOrderStatusCommand, CommandResult>;
