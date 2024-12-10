using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Data;
using MyFirstApi.Models;
using MyFirstApi.Services;
using System.Security.Cryptography;
using System.Text;

namespace MyFirstApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase 
    {
        private readonly ApiDbContext _context;
        private readonly AuthService _authService;

        public LoginController(ApiDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] Users user)
        {
            if(user == null) return Unauthorized(new { message = "Invalid credentials" });

            var hashedPassword = HashPassword(user.password);
            var existingUser = _context.Users.FirstOrDefault(u => 
                u.username == user.username && 
                u.password == hashedPassword);

            if(existingUser == null) return Unauthorized(new { message = "Invalid credentials" });

            var token = _authService.GenerateToken(user.username);
            Response.Cookies.Append("authToken", token, new CookieOptions 
            { 
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return Ok(new { message = "Login successful" });
        }


        [HttpGet("check-auth")]
        public IActionResult CheckAuth()
        {
            var token = Request.Cookies["authToken"];
            if(string.IsNullOrEmpty(token)) return Unauthorized();

            var username = _authService.GetUsernameFromToken(token);
            if(username == null) return Unauthorized();

            return Ok(new { authenticated = true, username });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("authToken");
            return Ok(new { message = "Logged out successfully" });
        }   

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}