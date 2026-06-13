using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.Patients.Commands.CreatePatient;

public sealed record CreatePatientCommand(
    string Mrn,
    string FullName,
    DateOnly? DateOfBirth,
    string? Sex,
    string Status) : ICommand<CommandResult>;

public interface ICreatePatientCommandHandler : ICommandHandler<CreatePatientCommand, CommandResult>;
