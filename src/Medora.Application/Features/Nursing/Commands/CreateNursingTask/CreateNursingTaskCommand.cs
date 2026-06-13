using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.Nursing.Commands.CreateNursingTask;

public sealed record CreateNursingTaskCommand(
    string TaskNumber,
    string PatientName,
    string WardName,
    string TaskType,
    DateTimeOffset DueAt,
    string NurseName) : ICommand<CommandResult>;

public interface ICreateNursingTaskCommandHandler : ICommandHandler<CreateNursingTaskCommand, CommandResult>;
