namespace Medora.Application.Modules.TenantAdministration.Users;

public interface ITenantUserOnboardingService
{
    Task<IReadOnlyList<AssignableRoleResponse>> GetAssignableRolesAsync(CancellationToken cancellationToken = default);

    Task<UserOnboardingResult> OnboardUserAsync(
        Guid tenantId,
        UserOnboardingRequest request,
        CancellationToken cancellationToken = default);
}

public sealed record AssignableRoleResponse(string Code, string Name, string Description);

public sealed record UserOnboardingRequest(
    string FullName,
    string Email,
    IReadOnlyList<string> RoleCodes,
    string? EmployeeNumber,
    string? Department,
    string? JobTitle,
    string? EmploymentType,
    DateOnly? StartDate);

public sealed record UserOnboardingResult(
    Guid UserId,
    Guid StaffMemberId,
    string EmployeeNumber,
    string Email,
    string TemporaryPassword,
    IReadOnlyList<AssignableRoleResponse> Roles);
