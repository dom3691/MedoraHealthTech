using Medora.Application.Common.Models;
using Medora.Application.Screens;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public sealed class DashboardController(IScreenCatalog screenCatalog) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<ScreenDefinition>>> GetDashboard(
        [FromQuery] string? tenantCode,
        CancellationToken cancellationToken)
    {
        var dashboard = await screenCatalog.GetScreenAsync("dashboard", tenantCode, cancellationToken);
        return dashboard is null
            ? NotFound(ApiResponse<ScreenDefinition>.Fail("Dashboard screen was not found for the selected tenant."))
            : Ok(ApiResponse<ScreenDefinition>.Ok(dashboard));
    }
}


