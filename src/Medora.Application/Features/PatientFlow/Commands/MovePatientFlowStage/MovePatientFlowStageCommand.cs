using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.PatientFlow.Commands.MovePatientFlowStage;

public sealed record MovePatientFlowStageCommand(Guid EncounterId, string Stage, string Status) : ICommand<CommandResult>;

public interface IMovePatientFlowStageCommandHandler : ICommandHandler<MovePatientFlowStageCommand, CommandResult>;
