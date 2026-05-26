namespace TrainingManagement.AuthenticationApi.Configurations;

/// <summary>
/// CORS configuration extension for WebApplicationBuilder
/// </summary>
public static class CorsConfig
{
    public static WebApplicationBuilder AddCorsConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Development", policyBuilder =>
            {
                policyBuilder.AllowAnyMethod()
                             .AllowAnyHeader()
                             .AllowAnyOrigin()
                             .AllowCredentials();
            });

            options.AddPolicy("Production", policyBuilder =>
            {
                policyBuilder.AllowAnyMethod()
                             .AllowAnyHeader()
                             .AllowAnyOrigin()
                             .AllowCredentials();
            });
        });

        return builder;
    }
}
