using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FirstApi.Models;

namespace FirstApi.Services
{
    public class JwtTokenServices
    {
        private readonly IConfiguration _Configuration;

        public JwtTokenServices(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }

        public string GenerateJwtToken(User user)
        {
            var jwtSettings = _Configuration.GetSection("JwtSettings");
            var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Email, user.Email),
            }),
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings["AccessTokenExpiration"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(securityTokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
