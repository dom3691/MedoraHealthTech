using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.RadiologyImaging.Commands.CreateRadiologyStudy;

public sealed record CreateRadiologyStudyCommand(
    string StudyNumber,
    string PatientName,
    string Modality,
    string BodyPart,
    string? Radiologist) : ICommand<CommandResult>;

public interface ICreateRadiologyStudyCommandHandler : ICommandHandler<CreateRadiologyStudyCommand, CommandResult>;
