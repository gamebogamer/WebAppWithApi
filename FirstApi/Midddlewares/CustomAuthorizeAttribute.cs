using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using FirstApi.Services;
using FirstApi.Interfaces;
using Microsoft.AspNetCore.Identity;
using FirstApi.DTOs;
// using YourNamespace.Data; // Replace with your actual namespace

public class CustomAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (IsAllowAnonymous(context))
        {
            return;
        }

        var serviceProvider = context.HttpContext.RequestServices;

        var userRepository = serviceProvider.GetRequiredService<IUserRepository>();
        var jwtTokenServices = serviceProvider.GetRequiredService<JwtTokenServices>();
        var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher<LogInDTO>>();
        var activeTokenRepository = serviceProvider.GetRequiredService<IActiveTokenRepository>();
        var logger = serviceProvider.GetRequiredService<ILogger<CustomAuthorizeAttribute>>();

        var token = GetTokenFromHeader(context);
        if (string.IsNullOrEmpty(token))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var jwtToken = ValidateToken(token, logger);
        if (jwtToken == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

        if (!IsValidUser(userId, email))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!await IsTokenActiveAsync(userId, token, userRepository, activeTokenRepository, jwtTokenServices, passwordHasher))
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }

    private JwtSecurityToken ValidateToken(string token, ILogger logger)
    {
        var handler = new JwtSecurityTokenHandler();
        try
        {
            return handler.ReadToken(token) as JwtSecurityToken;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Token validation failed.");
            return null;
        }
    }

    private async Task<bool> IsTokenActiveAsync(
        string userId, string token, 
        IUserRepository userRepository, 
        IActiveTokenRepository activeTokenRepository, 
        JwtTokenServices jwtTokenServices, 
        IPasswordHasher<LogInDTO> passwordHasher)
    {
        if (!int.TryParse(userId, out int userIdInt))
        {
            return false;
        }

        var usersServices = new UsersServices(userRepository, activeTokenRepository, jwtTokenServices, passwordHasher);
        var activeToken = await usersServices.GetActiveTokenAsync(userIdInt);

        return activeToken != null;
    }

    private bool IsAllowAnonymous(AuthorizationFilterContext context)
    {
        return context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
    }

    private string GetTokenFromHeader(AuthorizationFilterContext context)
    {
        var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            return null;
        }

        return authorizationHeader.Substring("Bearer ".Length).Trim();
    }

    private bool IsValidUser(string userId, string email)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email))
        {
            return false;
        }

        return int.TryParse(userId, out _);
    }
}
