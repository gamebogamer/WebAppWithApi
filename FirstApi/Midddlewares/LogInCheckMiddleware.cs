using System.IdentityModel.Tokens.Jwt;
using FirstApi.Services;
using FirstApi.Interfaces;
using Microsoft.AspNetCore.Identity;
using FirstApi.DTOs;

namespace FirstApi.Middlewares
{

    public class ActiveTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public ActiveTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                await _next(context);
                return;
            }

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null)
            {
                await _next(context);
                return;
            }

            var userId = jsonToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var email = jsonToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email))
            {
                await _next(context);
                return;
            }

            var userRepository = context.RequestServices.GetService<IUserRepository>();
            var jwtTokenServices = context.RequestServices.GetService<JwtTokenServices>();
            var passwordHasher = context.RequestServices.GetService<IPasswordHasher<LogInDTO>>();
            var activeTokenRepository = context.RequestServices.GetService<IActiveTokenRepository>();
            UsersServices usersServices = new UsersServices(userRepository, activeTokenRepository, jwtTokenServices, passwordHasher);

            if (!int.TryParse(userId, out int userIdInt))
            {
                await _next(context);
                return;
            }

            var activeToken = await usersServices.GetActiveTokenAsync(userIdInt);

            if (activeToken == null || activeToken.UserToken != token)
            {
                await _next(context);
                return;
            }

            context.Items["IsLogIn"] = true;
            context.Items["UserId"] = userId;
            context.Items["Email"] = email;

            await _next(context);
        }
    }

    public static class ActiveTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseActiveTokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ActiveTokenMiddleware>();
        }
    }
}