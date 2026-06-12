namespace Medora.Application.Modules.Identity.Auth;

public interface IAuthService
{
    Task<AuthSessionResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}

public interface IJwtTokenService
{
    string CreateToken(TenantContext tenant, UserContext user);
}

public sealed record LoginRequest(string TenantCode, string Email, string Password);

public sealed record AuthSessionResponse(TenantContext Tenant, UserContext User, string Token);

public sealed record TenantContext(Guid Id, string Code, string Name, string? Location, int LicensedBeds, string Plan);

public sealed record UserContext(Guid Id, string FullName, string Email, IReadOnlyList<string> RoleCodes, IReadOnlyList<string> RoleNames);
