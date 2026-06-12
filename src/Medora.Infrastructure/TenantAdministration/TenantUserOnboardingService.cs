using System.Security.Cryptography;
using Medora.Application.Common.Exceptions;
using Medora.Application.Modules.TenantAdministration.Users;
using Medora.Application.Security;
using Medora.Domain.HrOperations;
using Medora.Domain.Identity;
using Medora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Medora.Infrastructure.TenantAdministration;

public sealed class TenantUserOnboardingService(MedoraDbContext dbContext) : ITenantUserOnboardingService
{
    public async Task<IReadOnlyList<AssignableRoleResponse>> GetAssignableRolesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Roles
            .AsNoTracking()
            .Where(x => x.Code != "MSA")
            .OrderBy(x => x.Name)
            .Select(x => new AssignableRoleResponse(x.Code, x.Name, x.Description))
            .ToListAsync(cancellationToken);
    }

    public async Task<UserOnboardingResult> OnboardUserAsync(
        Guid tenantId,
        UserOnboardingRequest request,
        CancellationToken cancellationToken = default)
    {
        var tenant = await dbContext.Tenants
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == tenantId && x.IsActive, cancellationToken);

        if (tenant is null)
        {
            throw new ValidationException("The active tenant could not be resolved.");
        }

        var email = request.Email.Trim().ToLowerInvariant();
        var fullName = request.FullName.Trim();
        var roleCodes = request.RoleCodes
            .Select(x => x.Trim().ToUpperInvariant())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) || roleCodes.Length == 0)
        {
            throw new ValidationException("Full name, email, and at least one role are required.");
        }

        if (roleCodes.Contains("MSA", StringComparer.OrdinalIgnoreCase))
        {
            throw new ValidationException("Medora Super Admin cannot be assigned inside a tenant.");
        }

        if (await dbContext.Users.AnyAsync(x => x.TenantId == tenant.Id && x.Email == email, cancellationToken))
        {
            throw new ValidationException($"A user with email '{email}' already exists in this tenant.");
        }

        var roles = await dbContext.Roles
            .Where(x => roleCodes.Contains(x.Code))
            .ToListAsync(cancellationToken);

        var missingRoles = roleCodes.Except(roles.Select(x => x.Code), StringComparer.OrdinalIgnoreCase).ToArray();
        if (missingRoles.Length > 0)
        {
            throw new ValidationException($"Unknown role code(s): {string.Join(", ", missingRoles)}.");
        }

        var employeeNumber = string.IsNullOrWhiteSpace(request.EmployeeNumber)
            ? await GenerateEmployeeNumberAsync(tenant.Id, cancellationToken)
            : request.EmployeeNumber.Trim().ToUpperInvariant();

        if (await dbContext.StaffMembers.AnyAsync(x => x.TenantId == tenant.Id && x.EmployeeNumber == employeeNumber, cancellationToken))
        {
            throw new ValidationException($"Employee number '{employeeNumber}' already exists in this tenant.");
        }

        var now = DateTimeOffset.UtcNow;
        var temporaryPassword = GenerateTemporaryPassword();
        var user = new ApplicationUser
        {
            TenantId = tenant.Id,
            CreatedAt = now,
            FullName = fullName,
            Email = email,
            PasswordHash = PasswordHasher.Hash(tenant.Code, email, temporaryPassword),
            IsActive = true
        };

        foreach (var role in roles)
        {
            user.UserRoles.Add(new UserRole { User = user, Role = role });
        }

        var staffMember = new StaffMember
        {
            TenantId = tenant.Id,
            CreatedAt = now,
            EmployeeNumber = employeeNumber,
            FullName = fullName,
            Department = string.IsNullOrWhiteSpace(request.Department) ? "Unassigned" : request.Department.Trim(),
            JobTitle = string.IsNullOrWhiteSpace(request.JobTitle) ? "Employee" : request.JobTitle.Trim(),
            EmploymentType = string.IsNullOrWhiteSpace(request.EmploymentType) ? "Employee" : request.EmploymentType.Trim(),
            Status = "Active",
            WorkEmail = email,
            StartDate = request.StartDate
        };

        dbContext.Users.Add(user);
        dbContext.StaffMembers.Add(staffMember);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UserOnboardingResult(
            user.Id,
            staffMember.Id,
            employeeNumber,
            email,
            temporaryPassword,
            roles.OrderBy(x => x.Name).Select(x => new AssignableRoleResponse(x.Code, x.Name, x.Description)).ToArray());
    }

    private async Task<string> GenerateEmployeeNumberAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        var count = await dbContext.StaffMembers.CountAsync(x => x.TenantId == tenantId, cancellationToken);
        return $"EMP-{count + 1:0000}";
    }

    private static string GenerateTemporaryPassword()
    {
        Span<byte> bytes = stackalloc byte[12];
        RandomNumberGenerator.Fill(bytes);
        return $"Medora-{Convert.ToBase64String(bytes).Replace("+", "", StringComparison.Ordinal).Replace("/", "", StringComparison.Ordinal).TrimEnd('=')}!";
    }
}
