using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.Telemedicine.Commands.ScheduleTelemedicineSession;

public sealed record ScheduleTelemedicineSessionCommand(
    string SessionNumber,
    string PatientName,
    string ProviderName,
    string Channel,
    DateTimeOffset ScheduledAt,
    int DurationMinutes) : ICommand<CommandResult>;

public interface IScheduleTelemedicineSessionCommandHandler : ICommandHandler<ScheduleTelemedicineSessionCommand, CommandResult>;
