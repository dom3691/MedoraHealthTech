using Medora.Application.Common.Models;
using Medora.Application.Screens;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/doctor-portal")]
public sealed class DoctorPortalController(IScreenCatalog screenCatalog) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<ScreenDefinition>>> GetPortal(
        [FromQuery] string? tenantCode,
        CancellationToken cancellationToken)
    {
        var portal = await screenCatalog.GetScreenAsync("doctor-portal", tenantCode, cancellationToken);
        return portal is null
            ? NotFound(ApiResponse<ScreenDefinition>.Fail("Doctor portal screen was not found for the selected tenant."))
            : Ok(ApiResponse<ScreenDefinition>.Ok(portal));
    }
}


