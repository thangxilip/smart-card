using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Options;

namespace SmartCard.Api.Startup;

public class OAuth2EventsPostConfigure : IPostConfigureOptions<ApplicationPartManager>
{
    public void PostConfigure(string? name, ApplicationPartManager options)
    {
        
    }
}