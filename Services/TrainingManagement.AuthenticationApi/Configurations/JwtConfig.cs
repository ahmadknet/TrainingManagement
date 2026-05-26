namespace TrainingManagement.AuthenticationApi.Configurations;

/// <summary>
/// JWT (JSON Web Token) configuration settings
/// </summary>
public class JwtConfig
{
    public string? Secret { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int ExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
}
