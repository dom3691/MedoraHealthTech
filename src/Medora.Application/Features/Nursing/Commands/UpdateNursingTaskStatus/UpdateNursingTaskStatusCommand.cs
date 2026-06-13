using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.Nursing.Commands.UpdateNursingTaskStatus;

public sealed record UpdateNursingTaskStatusCommand(Guid TaskId, string Status) : ICommand<CommandResult>;

public interface IUpdateNursingTaskStatusCommandHandler : ICommandHandler<UpdateNursingTaskStatusCommand, CommandResult>;
