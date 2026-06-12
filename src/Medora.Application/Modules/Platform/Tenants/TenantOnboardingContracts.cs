namespace Medora.Application.Modules.Platform.Tenants;

public interface IPlatformTenantOnboardingService
{
    Task<TenantOnboardingResult> OnboardAsync(TenantOnboardingRequest request, CancellationToken cancellationToken = default);
}

public sealed record TenantOnboardingRequest(
    string Name,
    string Code,
    string? Location,
    int LicensedBeds,
    string Plan,
    string AdminFullName,
    string AdminEmail,
    bool IncludeDemoOperationalData = false);

public sealed record TenantOnboardingResult(
    Guid TenantId,
    string TenantCode,
    string TenantName,
    Guid AdminUserId,
    string AdminEmail,
    string TemporaryPassword,
    bool IncludedDemoOperationalData);
