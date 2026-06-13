using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.TheatreAndSurgery.Commands.UpdateSurgeryStatus;

public sealed record UpdateSurgeryStatusCommand(Guid CaseId, string Status) : ICommand<CommandResult>;

public interface IUpdateSurgeryStatusCommandHandler : ICommandHandler<UpdateSurgeryStatusCommand, CommandResult>;
