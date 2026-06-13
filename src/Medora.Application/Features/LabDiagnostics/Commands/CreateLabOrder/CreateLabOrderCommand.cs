using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.LabDiagnostics.Commands.CreateLabOrder;

public sealed record CreateLabOrderCommand(
    string OrderNumber,
    string PatientName,
    string TestName,
    string Priority,
    string? AssignedTo) : ICommand<CommandResult>;

public interface ICreateLabOrderCommandHandler : ICommandHandler<CreateLabOrderCommand, CommandResult>;
