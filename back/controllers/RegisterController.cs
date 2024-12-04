using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Data;
using MyFirstApi.Models;
using System.Security.Cryptography;
using System.Text;

namespace MyFirstApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public RegisterController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] Users user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User data is required");
                }

                if (string.IsNullOrWhiteSpace(user.username) || string.IsNullOrWhiteSpace(user.password))
                {
                    return BadRequest("Username and password are required");
                }

                var existingUser = _context.Users.FirstOrDefault(u => u.username == user.username);
                if (existingUser != null)
                {
                    return BadRequest("Username already exists");
                }

                user.password = HashPassword(user.password);
                 var defaultBudget = new Budget
                {
                    CurrentAmount = 0,
                    MaxAmount = 1000, 
                };
                user.Budget = defaultBudget;
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Registration successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred during registration");
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}