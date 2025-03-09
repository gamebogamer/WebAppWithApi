using Microsoft.EntityFrameworkCore;
using FirstApi.Models;
using FirstApi.Interfaces;
using FirstApi.Repository;
using FirstApi.DTOs;
using FirstApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Text;
// using FirstApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Retrieve JWT settings from configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

// Add JWT Authentication middleware
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

// Add MVC controllers support
builder.Services.AddControllers();

// Enable API documentation generation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database connection with PostgreSQL
builder.Services.AddDbContext<MyDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repository and service dependencies for dependency injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UsersServices>();
builder.Services.AddScoped<IActiveTokenRepository, ActiveTokenRepository>();
builder.Services.AddSingleton<IPasswordHasher<LogInDTO>, PasswordHasher<LogInDTO>>();
builder.Services.AddScoped<JwtTokenServices, JwtTokenServices>();
// builder.Services.AddScoped<IActiveTokenServices,ActiveTokenServices>();

var app = builder.Build();

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();
// app.UseActiveTokenMiddleware();


// Configure middleware pipeline for development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Map API controllers
app.MapControllers();

// Run the application
app.Run();
