using Medora.Application.Common.Models;
using Medora.Application.Modules.Operations.Data;
using Medora.Application.Modules.Platform.Tenants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/tenants")]
public sealed class TenantsController(
    IOperationsDataQueryService operationsData,
    IPlatformTenantOnboardingService tenantOnboarding) : ControllerBase
{
    [HttpGet]
    [HttpGet("/api/operations/tenants")]
    public async Task<ActionResult<ApiResponse<object>>> GetTenants(CancellationToken cancellationToken)
    {
        var tenants = await operationsData.GetTenantsAsync(cancellationToken);
        return Ok(ApiResponse<object>.Ok(tenants));
    }

    [HttpPost]
    [HttpPost("/api/admin/tenants")]
    [Authorize(Policy = "MedoraSuperAdmin")]
    public async Task<ActionResult<ApiResponse<TenantOnboardingResult>>> OnboardTenant(
        [FromBody] TenantOnboardingRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Code) ||
            string.IsNullOrWhiteSpace(request.Plan) ||
            string.IsNullOrWhiteSpace(request.AdminFullName) ||
            string.IsNullOrWhiteSpace(request.AdminEmail))
        {
            return BadRequest(ApiResponse<TenantOnboardingResult>.Fail("Tenant name, code, plan, admin name, and admin email are required."));
        }

        try
        {
            var result = await tenantOnboarding.OnboardAsync(request, cancellationToken);
            return Created($"/api/tenants/{result.TenantCode}", ApiResponse<TenantOnboardingResult>.Ok(result, "Tenant onboarded successfully."));
        }
        catch (Medora.Application.Common.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<TenantOnboardingResult>.Fail(ex.Message));
        }
    }
}
