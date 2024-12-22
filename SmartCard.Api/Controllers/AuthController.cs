using Microsoft.AspNetCore.Mvc;
using SmartCard.Application.Dtos.Auth;
using SmartCard.Domain.Interfaces;

namespace SmartCard.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IGoogleService googleService, IUserService userService, IJwtService jwtService) : ControllerBase
{
    [HttpGet("google/generate-login-url")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GenerateGoogleLoginUrl()
    {
        var url = await googleService.GetAuthorizationUrlAsync();
        return Ok(url);
    }
    
    [HttpGet("google/exchange-code")]
    [ProducesResponseType(typeof(LoginOutput), StatusCodes.Status200OK)]
    public async Task<IActionResult> ExchangeCodeAsync(string code)
    {
        var userInfo = await googleService.AuthenticateAsync(code);
        await userService.CreateExternalUserIfNotExistAsync(userInfo);
        var accessToken = jwtService.GenerateJwtToken(userInfo);
        return Ok(new LoginOutput
        {
            AccessToken = accessToken
        });
    }
}