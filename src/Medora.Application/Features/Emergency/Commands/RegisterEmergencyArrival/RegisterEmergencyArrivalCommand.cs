using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.Emergency.Commands.RegisterEmergencyArrival;

public sealed record RegisterEmergencyArrivalCommand(
    string CaseNumber,
    string PatientName,
    string Acuity,
    string Complaint) : ICommand<CommandResult>;

public interface IRegisterEmergencyArrivalCommandHandler : ICommandHandler<RegisterEmergencyArrivalCommand, CommandResult>;
