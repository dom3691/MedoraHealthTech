using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.TheatreAndSurgery.Commands.ScheduleSurgery;

public sealed record ScheduleSurgeryCommand(
    string CaseNumber,
    string PatientName,
    string Procedure,
    string SurgeonName,
    string Theatre,
    DateTimeOffset ScheduledAt) : ICommand<CommandResult>;

public interface IScheduleSurgeryCommandHandler : ICommandHandler<ScheduleSurgeryCommand, CommandResult>;
