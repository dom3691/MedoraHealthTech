using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.PatientFlow.Commands.RegisterWalkIn;

public sealed record RegisterWalkInCommand(
    string EncounterNumber,
    string? PatientMrn,
    string PatientName,
    string Complaint,
    string? PayerType,
    string? Insurer) : ICommand<CommandResult>;

public interface IRegisterWalkInCommandHandler : ICommandHandler<RegisterWalkInCommand, CommandResult>;
