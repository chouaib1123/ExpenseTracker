using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.Models;
using MyFirstApi.Services;

namespace MyFirstApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly AuthService _authService;

        public ExpenseController(ApiDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public class ExpenseDTO
        {
            public int Id { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
            public string Category { get; set; } = string.Empty;
        }

        [HttpGet]
        public  IActionResult  GetAllExpenses()
        {
            try
            {
                var username =  _authService.ValidateTokenFromRequest(Request);
                if (username == null)
                    return Unauthorized(new { message = "Invalid token" });

                var expenses =  _context.Expenses
                    .Where(e => e.User.username == username)
                    .Select(e => new ExpenseDTO
                    {
                        Id = e.Id,
                        Amount = e.Amount,
                        Date = e.Date,
                        Category = e.Category
                    })
                    .OrderByDescending(e => e.Date)
                    .ToList();

                if (!expenses.Any())
                    return NotFound(new { message = "No expenses found" });

                return Ok(expenses);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving expenses: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


        [HttpPost]
        public IActionResult CreateExpense([FromBody] ExpenseDTO expenseDto)
        {
            try
            {
                var username = _authService.ValidateTokenFromRequest(Request);
                if (username == null)
                    return Unauthorized(new { message = "Invalid token" });

                var user =  _context.Users
                    .Include(u => u.Budget)
                    .FirstOrDefault(u => u.username == username);

                if (user == null)
                    return NotFound(new { message = "User not found" });
                
                if (user.Budget == null)
                    return NotFound(new { message = "Budget not found" });

                var expense = new Expenses
                {
                    Amount = expenseDto.Amount,
                    Date = expenseDto.Date,
                    Category = expenseDto.Category,
                    UserId = user.Id
                };
                user.Budget.CurrentAmount += expense.Amount;

                _context.Expenses.Add(expense);
                _context.SaveChanges();

                return Ok(new { 
                    message = "Expense added successfully",
                    newBudget = user.Budget.CurrentAmount,
                    expense.Id,
                    expense.Amount,
                    expense.Date,
                    expense.Category
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating expense: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpDelete("{id}")]
        public  IActionResult DeleteExpense(int id)
        {
            try
            {
                var username = _authService.ValidateTokenFromRequest(Request);
                if (username == null)
                    return Unauthorized(new { message = "Invalid token" });

                var expense =  _context.Expenses
                    .Include(e => e.User)
                    .Include(e => e.User.Budget)
                    .FirstOrDefault(e => e.Id == id && e.User.username == username);

                if (expense == null)
                    return NotFound(new { message = "Expense not found" });    

                var user = _context.Users.FirstOrDefault(e => e.username == username);

                user.Budget.MaxAmount -= expense.Amount;

                _context.Expenses.Remove(expense);
                _context.SaveChanges();

                return Ok(new { message = "Expense deleted successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting expense: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}