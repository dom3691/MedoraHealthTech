using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.Outpatient.Commands.BookOutpatientVisit;

public sealed record BookOutpatientVisitCommand(
    string AppointmentNumber,
    string PatientMrn,
    string PatientName,
    string ProviderName,
    string Department,
    string VisitType,
    DateTimeOffset ScheduledAt,
    string? Room) : ICommand<CommandResult>;

public interface IBookOutpatientVisitCommandHandler : ICommandHandler<BookOutpatientVisitCommand, CommandResult>;
