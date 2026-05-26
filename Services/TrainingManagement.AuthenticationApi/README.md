# Training Management Authentication API

A .NET 10 Core Web API project for handling user authentication and authorization in the Training Management system.

## Project Structure

```
TrainingManagement.AuthenticationApi/
├── Configurations/          # Configuration classes for various aspects
│   ├── ApiConfig.cs        # API metadata configuration
│   ├── CorsConfig.cs       # CORS settings
│   ├── DbContextConfig.cs  # Database context configuration
│   ├── IdentityConfig.cs   # Identity and password policies
│   ├── JwtConfig.cs        # JWT token configuration
│   └── SwaggerConfig.cs    # Swagger/OpenAPI documentation settings
├── Controllers/            # API endpoint controllers
│   └── AuthController.cs   # Authentication endpoints (login, refresh)
├── Data/                   # Data access layer
│   └── AuthenticationDbContext.cs  # Entity Framework DbContext
├── Models/                 # Data models and DTOs
│   ├── User.cs            # User entity model
│   ├── Role.cs            # Role entity model
│   ├── LoginRequest.cs    # Login request DTO
│   └── LoginResponse.cs   # Login response DTO with UserDto
├── appsettings.json       # Configuration settings
├── Program.cs             # Application startup configuration
└── TrainingManagement.AuthenticationApi.csproj  # Project file
```

## Features

- **User Authentication**: Login endpoint for user authentication
- **JWT Token Management**: JWT token generation and refresh capabilities
- **Role-Based Authorization**: Support for user roles and permissions
- **Database Context**: Entity Framework Core integration with SQL Server support
- **CORS Support**: Configured CORS policies for cross-origin requests
- **Swagger Documentation**: OpenAPI/Swagger integration for API documentation
- **In-Memory Database**: Fallback support for in-memory database during development

## Technologies

- **.NET 10.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 10.0**
- **JWT Authentication**
- **SQL Server**
- **Swagger/OpenAPI**

## NuGet Dependencies

- `Swashbuckle.AspNetCore` - Swagger documentation
- `Microsoft.EntityFrameworkCore` - ORM framework
- `Microsoft.EntityFrameworkCore.SqlServer` - SQL Server provider
- `Microsoft.EntityFrameworkCore.InMemory` - In-memory database
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` - Identity management
- `System.IdentityModel.Tokens.Jwt` - JWT token handling

## Configuration

Settings are configured in `appsettings.json`:

### JWT Configuration
- **Secret**: Token signing key (change in production)
- **Issuer**: Token issuer identifier
- **Audience**: Token audience
- **ExpirationMinutes**: Access token expiration time
- **RefreshTokenExpirationDays**: Refresh token expiration period

### Identity Configuration
- **PasswordMinLength**: Minimum password length
- **RequireDigit**: Require numeric characters
- **RequireNonAlphanumeric**: Require special characters
- **RequireUppercase**: Require uppercase letters
- **RequireLowercase**: Require lowercase letters

### Database Configuration
- **ConnectionString**: SQL Server connection string
- Default: Uses in-memory database if connection string is not provided

## API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/refresh` - Refresh access token

## Getting Started

1. **Build the project**
   ```powershell
   dotnet build
   ```

2. **Restore dependencies**
   ```powershell
   dotnet restore
   ```

3. **Run the API**
   ```powershell
   dotnet run
   ```

4. **Access Swagger UI**
   - Navigate to `https://localhost:5001/swagger/index.html`

## Development Notes

- **TODO**: Implement actual authentication logic in AuthController
- **TODO**: Integrate with Identity system for user management
- **TODO**: Implement token generation and validation
- **TODO**: Add database migrations

## Security Considerations

⚠️ **Important**: The JWT secret in `appsettings.json` must be changed for production deployments. Use a strong, secure key stored in environment variables or Azure Key Vault.

## Contributing

This is part of the Training Management system. Follow the project's coding standards and conventions when making changes.

## License

Please refer to the main repository license.
