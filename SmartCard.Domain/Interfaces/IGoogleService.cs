using SmartCard.Domain.Models.Auth;

namespace SmartCard.Domain.Interfaces;

public interface IGoogleService
{
    Task<string> GetAuthorizationUrlAsync();

    Task<UserInfo> AuthenticateAsync(string authorizationCode);
}