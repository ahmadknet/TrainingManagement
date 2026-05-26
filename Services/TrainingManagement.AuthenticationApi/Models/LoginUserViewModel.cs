using System.ComponentModel.DataAnnotations;

namespace TrainingManagement.AuthenticationApi.Models;

/// <summary>
/// Login request model
/// </summary>
public class LoginUserViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public required string Password { get; set; }
}
