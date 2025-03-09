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
        // Check if the action or controller has the [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata
            .OfType<AllowAnonymousAttribute>().Any();

        if (allowAnonymous)
        {
            return; // Skip authorization
        }


        // var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
        var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var token = authorizationHeader.Substring("Bearer ".Length).Trim();
        var handler = new JwtSecurityTokenHandler();

        try
        {
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            if (jwtToken == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;


            // var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userId == null || email == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }            

             if (!int.TryParse(userId, out int userIdInt))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>();
            var jwtTokenServices = context.HttpContext.RequestServices.GetService<JwtTokenServices>();
            var passwordHasher = context.HttpContext.RequestServices.GetService<IPasswordHasher<LogInDTO>>();
            var activeTokenRepository = context.HttpContext.RequestServices.GetService<IActiveTokenRepository>();
            UsersServices usersServices = new UsersServices(userRepository, activeTokenRepository, jwtTokenServices, passwordHasher);


            var activeToken = await usersServices.GetActiveTokenAsync(userIdInt);

            // Check if token is in ActiveToken table and not expired
            // var activeToken = dbContext.ActiveTokens
            //     .FirstOrDefault(t => t.UserId == userId && t.Token == token && t.Expiry > DateTime.UtcNow);

            if (activeToken == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
        catch
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}
