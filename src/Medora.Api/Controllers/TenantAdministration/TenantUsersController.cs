using Medora.Api.Security;
using Medora.Application.Common.Exceptions;
using Medora.Application.Modules.TenantAdministration.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers.TenantAdministration;

[ApiController]
[Route("api/admin/users")]
[Authorize(Policy = "TenantUserAdmin")]
public sealed class TenantUsersController(ITenantUserOnboardingService userOnboardingService) : ControllerBase
{
    [HttpGet("roles")]
    public async Task<ActionResult<IReadOnlyList<AssignableRoleResponse>>> GetAssignableRoles(CancellationToken cancellationToken)
    {
        var roles = await userOnboardingService.GetAssignableRolesAsync(cancellationToken);
        return Ok(roles);
    }

    [HttpPost]
    public async Task<ActionResult<UserOnboardingResult>> OnboardUser(
        [FromBody] UserOnboardingRequest request,
        CancellationToken cancellationToken)
    {
        var tenantId = User.GetTenantId();
        if (tenantId is null)
        {
            return Forbid();
        }

        try
        {
            var result = await userOnboardingService.OnboardUserAsync(tenantId.Value, request, cancellationToken);
            return Created($"/api/admin/users/{result.UserId}", result);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
