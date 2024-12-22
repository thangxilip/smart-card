using Microsoft.AspNetCore.Mvc;
using SmartCard.Domain.Interfaces;
using SmartCard.Domain.Models.User;

namespace SmartCard.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("{email}")]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmailAsync(string email)
    {
        var result = await userService.GetUserByEmailAsync(email);
        return Ok(result);
    }
}