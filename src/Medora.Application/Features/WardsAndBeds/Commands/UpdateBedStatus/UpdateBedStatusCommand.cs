using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.WardsAndBeds.Commands.UpdateBedStatus;

public sealed record UpdateBedStatusCommand(Guid BedId, string Status) : ICommand<CommandResult>;

public interface IUpdateBedStatusCommandHandler : ICommandHandler<UpdateBedStatusCommand, CommandResult>;
