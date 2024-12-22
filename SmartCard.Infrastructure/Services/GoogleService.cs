using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.Extensions.Options;
using SmartCard.Domain.Configurations;
using SmartCard.Domain.Interfaces;
using SmartCard.Domain.Models.Auth;

namespace SmartCard.Infrastructure.Services;

public class GoogleService(IOptions<GoogleSettings> options) : IGoogleService
{
    private readonly GoogleAuthorizationCodeFlow _flow = new(new GoogleAuthorizationCodeFlow.Initializer
    {
        ClientSecrets = new ClientSecrets
        {
            ClientId = options.Value.ClientId,
            ClientSecret = options.Value.ClientSecret,
        },
        Scopes = ["email", "profile"]
    });

    public Task<string> GetAuthorizationUrlAsync()
    {
        return Task.FromResult(
            _flow.CreateAuthorizationCodeRequest($"{options.Value.RedirectUri}").Build().AbsoluteUri
        );
    }

    public async Task<UserInfo> AuthenticateAsync(string authorizationCode)
    {
        var tokenResponse = await _flow.ExchangeCodeForTokenAsync(
            userId: "user",
            code: authorizationCode,
            redirectUri: options.Value.RedirectUri,
            CancellationToken.None
        );
        
        var payload = await GoogleJsonWebSignature.ValidateAsync(tokenResponse.IdToken);

        return new UserInfo
        {
            Email = payload.Email,
            FirstName = payload.GivenName,
            LastName = payload.FamilyName,
            Picture = payload.Picture
        };
    }
}