namespace SmartCard.Domain.Configurations;

public class AppConfig
{
    public ConnectionStrings ConnectionStrings { get; set; }

    public GoogleSettings Google { get; set; }

    public JwtSettings Jwt { get; set; }
}