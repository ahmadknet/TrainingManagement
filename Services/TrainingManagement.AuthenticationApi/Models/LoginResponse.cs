namespace TrainingManagement.AuthenticationApi.Models;

/// <summary>
/// Login response model
/// </summary>
public class LoginResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public UserDto? User { get; set; }
}

/// <summary>
/// User data transfer object
/// </summary>
public class UserDto
{
    public int Id { get; set; }
    public string? Email { get; set; }
}
