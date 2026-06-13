using Medora.Application.Modules.Identity.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthSessionResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var session = await authService.LoginAsync(request, cancellationToken);
        return session is null ? Unauthorized() : Ok(session);
    }
}


