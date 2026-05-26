using TrainingManagement.AuthenticationApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TrainingManagement.AuthenticationApi.Configurations;

/// <summary>
/// JWT (JSON Web Token) configuration settings
/// </summary>
public static class JwtConfig
{

    public static WebApplicationBuilder AddJwtConfig(this WebApplicationBuilder builder)
    {
        IConfiguration jwtSettingSection = builder.Configuration.GetSection("JwtSettings");
        _ = builder.Services.Configure<JwtSettings>(jwtSettingSection);

        JwtSettings jwtSettings = jwtSettingSection.Get<JwtSettings>() ?? throw new InvalidOperationException("JWT settings not found in configuration.");

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(jwtSettings.Secret);
        _ = builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

        });
        return builder;
        //.AddJwtBearer(options =>
        //{
        //    options.RequireHttpsMetadata = false;
        //    options.SaveToken = true;
        //    options.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidIssuer = jwtSettings.Issuer,
        //        ValidateAudience = true,
        //        ValidAudience = jwtSettings.Audience,
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        //        ValidateLifetime = true,
        //        ClockSkew = TimeSpan.Zero
        //    };
        //});

    }
}
