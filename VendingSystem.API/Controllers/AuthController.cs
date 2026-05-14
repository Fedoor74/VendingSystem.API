using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendingSystem.API.Data;
using VendingSystem.API.DTOs;
using VendingSystem.API.Models;
using VendingSystem.API.Services;
using BCrypt.Net;

namespace VendingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public AuthController(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest(new { message = "Пользователь с таким email уже существует." });

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Contacts = request.Phone, // Сохраняем телефон в поле Contacts
                Role = "Оператор",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Пользователь зарегистрирован" });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized(new { message = "Неверный email или пароль." });

            // Генерация JWT токена (Шаг 4 из PDF)
            var token = _authService.GenerateToken(user);

            return Ok(new AuthResponse(token));
        }
    }
}