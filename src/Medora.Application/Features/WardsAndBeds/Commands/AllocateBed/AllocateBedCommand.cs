using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.WardsAndBeds.Commands.AllocateBed;

public sealed record AllocateBedCommand(Guid BedId, string PatientMrn) : ICommand<CommandResult>;

public interface IAllocateBedCommandHandler : ICommandHandler<AllocateBedCommand, CommandResult>;
