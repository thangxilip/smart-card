using SmartCard.Domain.Models.Auth;

namespace SmartCard.Domain.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(UserInfo userInfo);
}