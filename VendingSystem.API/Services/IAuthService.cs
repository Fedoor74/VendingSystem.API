using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using VendingSystem.API.Models;

namespace VendingSystem.API.Services
{
    public interface IAuthService
    {
        string GenerateToken(User user);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key is missing");
            var issuer = jwtSettings["Issuer"] ?? "VendingAPI";
            var audience = jwtSettings["Audience"] ?? "VendingClient";

            if (!int.TryParse(jwtSettings["ExpiryMinutes"], out int expiryMinutes))
                expiryMinutes = 60;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Шаг 2 из PDF: Создаем PAYLOAD
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // Используем UserId
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("FullName", user.FullName)
            };

            // Шаг 3 из PDF: Создаем SIGNATURE
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            // Шаг 4 из PDF: Объединяем компоненты
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}