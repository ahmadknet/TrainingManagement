using TrainingManagement.AuthenticationApi.Data;
using TrainingManagement.AuthenticationApi.Configurations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<AuthenticationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        // Use in-memory database for development
        options.UseInMemoryDatabase("AuthenticationDb");
    }
    else
    {
        options.UseSqlServer(connectionString);
    }
});

// Add CORS Configuration
builder.AddCorsConfig();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Development");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
