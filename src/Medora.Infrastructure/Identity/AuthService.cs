using Medora.Application.Modules.Identity.Auth;
using Medora.Application.Security;
using Medora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Medora.Infrastructure.Identity;

public sealed class AuthService(MedoraDbContext dbContext, IJwtTokenService tokenService) : IAuthService
{
    public async Task<AuthSessionResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var tenant = await dbContext.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == request.TenantCode && x.IsActive, cancellationToken);

        if (tenant is null)
        {
            return null;
        }

        var user = await dbContext.Users
            .AsNoTracking()
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(x => x.TenantId == tenant.Id && x.Email == request.Email && x.IsActive, cancellationToken);

        if (user?.PasswordHash is null || !PasswordHasher.Verify(tenant.Code, user.Email, request.Password, user.PasswordHash))
        {
            return null;
        }

        var roleCodes = user.UserRoles.Select(x => x.Role.Code).ToArray();
        var roleNames = user.UserRoles.Select(x => x.Role.Name).ToArray();
        var tenantContext = new TenantContext(tenant.Id, tenant.Code, tenant.Name, tenant.Location, tenant.LicensedBeds, tenant.Plan);
        var userContext = new UserContext(user.Id, user.FullName, user.Email, roleCodes, roleNames);

        return new AuthSessionResponse(tenantContext, userContext, tokenService.CreateToken(tenantContext, userContext));
    }
}
