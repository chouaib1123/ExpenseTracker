using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.Models;
using MyFirstApi.Services;
using System;
using System.Text;

namespace MyFirstApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IAuthService _authService;

        public BudgetController(ApiDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public class BudgetDTO
        {
            public decimal CurrentAmount { get; set; }
            public decimal Budget { get; set; }
        }
        [HttpGet]
        public async Task<ActionResult<BudgetDTO>> Get()
        {
            try
            {
                var username = await _authService.ValidateTokenFromRequest(Request);
                if (username == null)
                    return Unauthorized(new { message = "Invalid token" });

                var user = await _context.Users
                    .Include(u => u.Budget)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.username == username);

                if (user?.Budget == null)
                    return NotFound(new { message = "Budget not found" });

                return Ok(new BudgetDTO
                {
                    CurrentAmount = user.Budget.CurrentAmount,
                    Budget = user.Budget.MaxAmount
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Get Budget: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateMaxBudget([FromBody] decimal maxAmount)
        {
            try
            {
                var username = await _authService.ValidateTokenFromRequest(Request);
                if (username == null)
                    return Unauthorized(new { message = "Invalid token" });

                var user = await _context.Users
                    .Include(u => u.Budget)
                    .FirstOrDefaultAsync(u => u.username == username);

                if (user == null)
                    return NotFound(new { message = "User not found" });
                
                if (user.Budget == null)
                    return NotFound(new { message = "Budget not found" });

                var oldMax = user.Budget.MaxAmount;
                user.Budget.MaxAmount = maxAmount;
                _context.Entry(user.Budget).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok(new { 
                    message = "Budget updated successfully",
                    oldMax = oldMax,
                    newMax = user.Budget.MaxAmount
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating budget: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}