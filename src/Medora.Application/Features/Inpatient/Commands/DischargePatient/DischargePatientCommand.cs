using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.Inpatient.Commands.DischargePatient;

public sealed record DischargePatientCommand(Guid AdmissionId) : ICommand<CommandResult>;

public interface IDischargePatientCommandHandler : ICommandHandler<DischargePatientCommand, CommandResult>;
