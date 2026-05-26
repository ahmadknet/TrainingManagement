namespace TrainingManagement.AuthenticationApi.Configurations;

/// <summary>
/// Identity configuration settings
/// </summary>
public class IdentityConfig
{
    public int PasswordMinLength { get; set; }
    public bool RequireDigit { get; set; }
    public bool RequireNonAlphanumeric { get; set; }
    public bool RequireUppercase { get; set; }
    public bool RequireLowercase { get; set; }
}
