using Medora.Application.Common.Interfaces;

namespace Medora.Api.Services;

public sealed class CurrentTenantService(IHttpContextAccessor httpContextAccessor) : ICurrentTenantService
{
    public Guid? TenantId
    {
        get
        {
            var rawTenantId = httpContextAccessor.HttpContext?.User.FindFirst("tenant_id")?.Value;
            return Guid.TryParse(rawTenantId, out var tenantId) ? tenantId : null;
        }
    }

    public string? TenantCode => httpContextAccessor.HttpContext?.User.FindFirst("tenant_code")?.Value;
}
