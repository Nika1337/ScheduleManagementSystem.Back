

namespace Infrastructure.Identity;

public class JwtSettings
{
    public string Secret { get; private init; }
    public string Issuer { get; private init; }
    public string Audience { get; private init; }
    public int ExpiryMinutes { get; private init; }

    public JwtSettings(
        string secret = "", 
        string issuer = "", 
        string audience = "",
        int expiryMinutes = 120)
    {
        Secret = secret;
        Issuer = issuer;
        Audience = audience;
        ExpiryMinutes = expiryMinutes;
    }
}
