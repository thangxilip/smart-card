namespace SmartCard.Domain.Configurations;

public class JwtSettings
{
    public string Key { get; set; }
    
    public string Issuer { get; set; }
    
    public string Audience { get; set; }
    
    public int AccessTokenExpireInHours { get; set; }
    
    public int RefreshTokenExpireInHours { get; set; }
}