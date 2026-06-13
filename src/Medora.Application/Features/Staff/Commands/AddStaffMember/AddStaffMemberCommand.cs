using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.Staff.Commands.AddStaffMember;

public sealed record AddStaffMemberCommand(
    string EmployeeNumber,
    string FullName,
    string Department,
    string JobTitle,
    string EmploymentType,
    string Status,
    string? WorkEmail,
    DateOnly? StartDate) : ICommand<CommandResult>;

public interface IAddStaffMemberCommandHandler : ICommandHandler<AddStaffMemberCommand, CommandResult>;
