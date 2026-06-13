using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/app/config")]
public sealed class AppConfigController(IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    public ActionResult<AppConfigResponse> Get()
    {
        var section = configuration.GetRequiredSection("Application");

        return Ok(new AppConfigResponse(
            Required(section, "ProductName"),
            Required(section, "ProductSuite"),
            Required(section, "DocumentTitle"),
            Required(section, "LoginHeroTitle"),
            Required(section, "LoginHeroSubtitle"),
            Required(section, "LoginComplianceText"),
            Required(section, "GlobalSearchPlaceholder")));
    }

    private static string Required(IConfiguration section, string key) =>
        section[key] ?? throw new InvalidOperationException($"Application configuration value '{key}' is not configured.");
}

public sealed record AppConfigResponse(
    string ProductName,
    string ProductSuite,
    string DocumentTitle,
    string LoginHeroTitle,
    string LoginHeroSubtitle,
    string LoginComplianceText,
    string GlobalSearchPlaceholder);


