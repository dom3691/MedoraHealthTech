using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.Appointments.Commands.CreateAppointment;

public sealed record CreateAppointmentCommand(
    string AppointmentNumber,
    string PatientMrn,
    string PatientName,
    string ProviderName,
    string Department,
    string VisitType,
    DateTimeOffset ScheduledAt,
    string? Room) : ICommand<CommandResult>;

public interface ICreateAppointmentCommandHandler : ICommandHandler<CreateAppointmentCommand, CommandResult>;
