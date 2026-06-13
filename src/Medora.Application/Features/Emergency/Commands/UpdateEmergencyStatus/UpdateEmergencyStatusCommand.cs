using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.Emergency.Commands.UpdateEmergencyStatus;

public sealed record UpdateEmergencyStatusCommand(Guid CaseId, string Status) : ICommand<CommandResult>;

public interface IUpdateEmergencyStatusCommandHandler : ICommandHandler<UpdateEmergencyStatusCommand, CommandResult>;
