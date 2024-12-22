using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartCard.Domain.Configurations;
using SmartCard.Domain.Interfaces;
using SmartCard.Domain.Models.Auth;

namespace SmartCard.Infrastructure.Services;

public class JwtService(IOptions<JwtSettings> options) : IJwtService
{
    public string GenerateJwtToken(UserInfo userInfo)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
            new Claim("firstName", userInfo.FirstName),
            new Claim("lastName", userInfo.LastName),
        };

        var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(options.Value.AccessTokenExpireInHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}