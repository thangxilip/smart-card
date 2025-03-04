using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SmartCard.Domain.Interfaces;

namespace SmartCard.Infrastructure.Services;

public class AppContextService : IAppContextService
{
    public AppContextService(IHttpContextAccessor httpContextAccessor, IUserService? userService)
    {
        var userClaims = httpContextAccessor.HttpContext?.User.Claims;
        if (userClaims is not null && userService is not null)
        {
            Email = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value!;
            var user = userService.GetUserByEmailAsync(Email).Result;
            UserId = user.Id;
        }
    }

    public Guid? UserId { get; set; }
    public string? Email { get; set; }
}