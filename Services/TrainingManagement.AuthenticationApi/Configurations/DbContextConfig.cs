using Microsoft.EntityFrameworkCore;
using TrainingManagement.AuthenticationApi.Data;

namespace TrainingManagement.AuthenticationApi.Configurations;

/// <summary>
/// Database context configuration settings
/// </summary>
public static class  DbContextConfig
{
    public static WebApplicationBuilder ConfigureDbContext(this WebApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<AuthenticationDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        return builder;
    }
}
