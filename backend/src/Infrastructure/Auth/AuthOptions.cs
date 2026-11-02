using System.Text;

namespace PremierLeagueWebApi.Infrastructure.Auth;

public sealed class AuthOptions
{
    public const string SectionName = "Auth";

    public JwtOptions Jwt { get; set; } = new();

    public static bool IsValid(AuthOptions options)
    {
        if (options is null
            || options.Jwt is null
            || string.IsNullOrWhiteSpace(options.Jwt.Issuer)
            || string.IsNullOrWhiteSpace(options.Jwt.Audience)
            || string.IsNullOrWhiteSpace(options.Jwt.SigningKey)
            || Encoding.UTF8.GetByteCount(options.Jwt.SigningKey) < 32
            || options.Jwt.ExpirationMinutes is < 1 or > 1440)
        {
            return false;
        }

        return true;
    }

    public const string ValidationError =
        "Auth configuration must include a JWT issuer, audience, signing key of at least 32 bytes, " +
        "and an expiry from 1 to 1440 minutes.";
}

public sealed class JwtOptions
{
    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string SigningKey { get; set; } = string.Empty;

    public int ExpirationMinutes { get; set; } = 60;
}
