using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueWebApi.API.Schemas.Auth;
using PremierLeagueWebApi.Core.Services;

namespace PremierLeagueWebApi.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthenticationService _authenticationService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var accessToken = await _authenticationService.AuthenticateAsync(request.Username, request.Password);
        return accessToken is null
            ? Unauthorized()
            : Ok(new LoginResponse(accessToken.Value, "Bearer", accessToken.ExpiresAtUtc));
    }
}
