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
        private readonly AuthService _authService;

        public BudgetController(ApiDbContext context, AuthService authService)
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
        public IActionResult GetBudget()
        {
            try
            {
                var username = _authService.ValidateTokenFromRequest(Request);
                if (username == null)
                    return Unauthorized(new { message = "Invalid token" });

                var user = _context.Users.FirstOrDefault(u => u.username == username);
                
                if (user == null)
                    return NotFound(new { message = "User not found" });

                var Budget = _context.Budget.FirstOrDefault(b => b.UserId == user.Id);    

                if (Budget == null)
                    return NotFound(new { message = "Budget not found for the user" });

                return Ok(
                    new BudgetDTO
                    {
                        CurrentAmount = Budget.CurrentAmount,
                        Budget = Budget.MaxAmount
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Get Budget: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }



        [HttpPost]
        public IActionResult UpdateMaxBudget([FromBody] decimal maxAmount)
        {
            try
            {
                var username = _authService.ValidateTokenFromRequest(Request);
                if (username == null)
                    return Unauthorized(new { message = "Invalid token" });

                var user = _context.Users
                    .Include(u => u.Budget)
                    .FirstOrDefault(u => u.username == username);

                if (user == null)
                    return NotFound(new { message = "User not found" });
                
                if (user.Budget == null)
                    return NotFound(new { message = "Budget not found" });
                user.Budget.MaxAmount = maxAmount;
             
                _context.SaveChangesAsync();

                return Ok(new { 
                    message = "Budget updated successfully",
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