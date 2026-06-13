using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.RadiologyImaging.Commands.UpdateRadiologyStudyStatus;

public sealed record UpdateRadiologyStudyStatusCommand(Guid StudyId, string Status, string? Finding) : ICommand<CommandResult>;

public interface IUpdateRadiologyStudyStatusCommandHandler : ICommandHandler<UpdateRadiologyStudyStatusCommand, CommandResult>;
