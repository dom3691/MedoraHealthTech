using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.Appointments.Commands.UpdateAppointmentStatus;

public sealed record UpdateAppointmentStatusCommand(Guid AppointmentId, string Status) : ICommand<CommandResult>;

public interface IUpdateAppointmentStatusCommandHandler : ICommandHandler<UpdateAppointmentStatusCommand, CommandResult>;
