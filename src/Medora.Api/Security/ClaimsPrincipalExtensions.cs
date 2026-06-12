using System.Security.Claims;

namespace Medora.Api.Security;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetTenantId(this ClaimsPrincipal principal)
    {
        var tenantId = principal.FindFirstValue("tenant_id");
        return Guid.TryParse(tenantId, out var parsed) ? parsed : null;
    }
}
