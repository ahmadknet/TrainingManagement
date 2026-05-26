namespace TrainingManagement.AuthenticationApi.Configurations;

/// <summary>
/// Database context configuration settings
/// </summary>
public class DbContextConfig
{
    public string? ConnectionString { get; set; }
    public bool EnableSensitiveDataLogging { get; set; }
    public bool UseInMemoryDatabase { get; set; }
}
