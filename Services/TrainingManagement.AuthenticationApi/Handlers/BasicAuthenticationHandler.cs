using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using TrainingManagement.AuthenticationApi.Data;
using TrainingManagement.AuthenticationApi.Models;

namespace TrainingManagement.AuthenticationApi.Handlers;

/// <summary>
/// Basic Authentication Handler for ASP.NET Core
/// </summary>
public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly ApplicationDbContext _authenticationDbContext;

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        ApplicationDbContext authenticationDbContext) 
        : base(options, logger, encoder, clock)
    {
        _authenticationDbContext = authenticationDbContext;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Can't authorize due to missing Authorization header");
        }

        try
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            var authHeaderValue = authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase)
                ? authHeader.Substring("Basic ".Length).Trim()
                : authHeader;

            var credentialsInBytes = Convert.FromBase64String(authHeaderValue);
            var credentials = Encoding.UTF8.GetString(credentialsInBytes);
            var email = credentials.Split(":")[0];
            var password = credentials.Split(":")[1];

            #region Validating with User defined DB
            var loginDetails = _authenticationDbContext.Users
                .Where(u => u.Email!.Equals(email))
                .FirstOrDefault();

            if (loginDetails == null)
            {
                return AuthenticateResult.Fail("Invalid credentials");
            }

            // Verify password (simple comparison for demo, use proper hashing in production)
            if (!VerifyPassword(password, loginDetails.PasswordHash))
            {
                return AuthenticateResult.Fail("Invalid credentials");
            }
            #endregion

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, loginDetails.Id.ToString()),
                new(ClaimTypes.Email, loginDetails.Email!)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Verifies a password against a hash
    /// </summary>
    private bool VerifyPassword(string password, string? hash)
    {
        if (string.IsNullOrEmpty(hash))
            return false;

        // Simple verification - in production use a proper hashing algorithm like BCrypt
        // For demo purposes, we compare directly (NOT RECOMMENDED FOR PRODUCTION)
        return password.Equals(hash);
    }
}
