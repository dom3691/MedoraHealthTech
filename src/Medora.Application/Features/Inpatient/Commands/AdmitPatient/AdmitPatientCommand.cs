using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.Inpatient.Commands.AdmitPatient;

public sealed record AdmitPatientCommand(
    string AdmissionNumber,
    string PatientMrn,
    string PatientName,
    string WardName,
    string? BedNumber,
    string AttendingProvider,
    string? Diagnosis,
    DateTimeOffset? DischargePlannedAt) : ICommand<CommandResult>;

public interface IAdmitPatientCommandHandler : ICommandHandler<AdmitPatientCommand, CommandResult>;
