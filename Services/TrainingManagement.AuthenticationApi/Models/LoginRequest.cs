namespace TrainingManagement.AuthenticationApi.Models;

/// <summary>
/// Login request model
/// </summary>
public class LoginRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}
