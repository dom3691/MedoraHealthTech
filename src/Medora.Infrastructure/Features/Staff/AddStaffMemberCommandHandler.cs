using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;
using Medora.Application.Features.Staff.Commands.AddStaffMember;
using Medora.Domain.HrOperations;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;

namespace Medora.Infrastructure.Features.Staff;

public sealed class AddStaffMemberCommandHandler(
    MedoraDbContext dbContext,
    ICurrentTenantService currentTenant,
    ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IAddStaffMemberCommandHandler
{
    public async Task<CommandResult> Handle(AddStaffMemberCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var staff = new StaffMember
        {
            EmployeeNumber = command.EmployeeNumber.Trim(),
            FullName = command.FullName.Trim(),
            Department = command.Department.Trim(),
            JobTitle = command.JobTitle.Trim(),
            EmploymentType = command.EmploymentType.Trim(),
            Status = command.Status.Trim(),
            WorkEmail = command.WorkEmail,
            StartDate = command.StartDate
        };

        StampCreate(staff, tenantId);
        DbContext.StaffMembers.Add(staff);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(staff, staff.EmployeeNumber, staff.Status);
    }
}
