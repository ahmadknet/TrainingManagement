using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrainingManagement.AuthenticationApi.Configurations;
using TrainingManagement.AuthenticationApi.Models;

namespace TrainingManagement.AuthenticationApi.Controllers;

/// <summary>
/// Authentication controller for login and token management
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings) : ControllerBase
{
    private readonly ILogger<AuthController> _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<AuthController>();
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    /// <summary>
    /// Login endpoint
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginUserViewModel request)
    {
        try
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Email and password are required"
                });
            }

            _logger.LogInformation("Login attempt for user: {Username}", request.Email);

            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(request.Email);
                if (user != null)
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new LoginResponse
                    {
                        Success = true,
                        Message = "Login successful",
                        AccessToken = token,
                        User = new UserDto
                        {
                            Id = int.Parse(user.Id),
                            Username = user.UserName,
                            Email = user.Email,
                            FirstName = user.Email?.Split('@')[0] ?? "User",
                            LastName = "User"
                        }
                    });
                }
            }

            return BadRequest(new LoginResponse
            {
                Success = false,
                Message = "Invalid username or password"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return StatusCode(500, new LoginResponse
            {
                Success = false,
                Message = "An error occurred during login"
            });
        }
    }

    /// <summary>
    /// Generate JWT token for authenticated user
    /// </summary>
    private string GenerateJwtToken(IdentityUser user)
    {
        var jwtConfig = _jwtSettings;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret ?? "default-secret-key-change-in-production"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? ""),
            new Claim(ClaimTypes.Email, user.Email ?? "")
        };

        var token = new JwtSecurityToken(
            issuer: jwtConfig.Issuer,
            audience: jwtConfig.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(jwtConfig.ExpirationHours), // use hours from JwtSettings
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Refresh token endpoint
    /// </summary>
    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] string refreshToken)
    {
        try
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Refresh token is required"
                });
            }

            _logger.LogInformation("Token refresh attempt");

            // Validate and extract claims from the refresh token
            var principal = GetPrincipalFromExpiredToken(refreshToken);
            if (principal == null)
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Invalid refresh token"
                });
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Invalid token claims"
                });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "User not found"
                });
            }

            var newToken = GenerateJwtToken(user);
            return Ok(new LoginResponse
            {
                Success = true,
                Message = "Token refreshed successfully",
                AccessToken = newToken,
                User = new UserDto
                {
                    Id = int.Parse(user.Id),
                    Username = user.UserName,
                    Email = user.Email,
                    FirstName = user.Email?.Split('@')[0] ?? "User",
                    LastName = "User"
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return StatusCode(500, new LoginResponse
            {
                Success = false,
                Message = "An error occurred during token refresh"
            });
        }
    }

    /// <summary>
    /// Register new user endpoint
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterUserViewModel registerUserViewModel)
    {
        try
        {
            if (registerUserViewModel == null || !ModelState.IsValid)
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Invalid registration data"
                });
            }

            if (string.IsNullOrEmpty(registerUserViewModel.Email) || string.IsNullOrEmpty(registerUserViewModel.Password))
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Email and password are required"
                });
            }

            if (registerUserViewModel.Password != registerUserViewModel.ConfirmPassword)
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Passwords do not match"
                });
            }

            var user = new IdentityUser
            {
                UserName = registerUserViewModel.Email ?? registerUserViewModel.Email,
                Email = registerUserViewModel.Email
            };

            var result = await _userManager.CreateAsync(user, registerUserViewModel.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User registered successfully: {Email}", registerUserViewModel.Email);

                var token = GenerateJwtToken(user);
                return Ok(new LoginResponse
                {
                    Success = true,
                    Message = "User registered successfully",
                    AccessToken = token,
                    User = new UserDto
                    {
                        Id = int.Parse(user.Id),
                        Username = user.UserName,
                        Email = user.Email
                    }
                });
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest(new LoginResponse
            {
                Success = false,
                Message = $"Registration failed: {errors}"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            return StatusCode(500, new LoginResponse
            {
                Success = false,
                Message = "An error occurred during registration"
            });
        }
    }

    /// <summary>
    /// Extract claims from an expired JWT token
    /// </summary>
    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        try
        {
            var jwtConfig = _jwtSettings;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret ?? "default-secret-key-change-in-production"));

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtConfig.Audience,
                ValidateLifetime = false // Allow expired tokens for refresh
            }, out SecurityToken securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
