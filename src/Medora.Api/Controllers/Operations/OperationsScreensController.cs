using Medora.Application.Screens;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers.Operations;

[ApiController]
[Route("api/operations/screens")]
public sealed class OperationsScreensController(IScreenCatalog screenCatalog) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ScreenCatalogResponse>> GetCatalog(
        [FromQuery] string? tenantCode,
        CancellationToken cancellationToken)
    {
        var catalog = await screenCatalog.GetCatalogAsync(tenantCode, cancellationToken);
        return Ok(catalog);
    }

    [HttpGet("{key}")]
    public async Task<ActionResult<ScreenDefinition>> GetScreen(
        string key,
        [FromQuery] string? tenantCode,
        CancellationToken cancellationToken)
    {
        var screen = await screenCatalog.GetScreenAsync(key, tenantCode, cancellationToken);
        return screen is null ? NotFound() : Ok(screen);
    }
}
