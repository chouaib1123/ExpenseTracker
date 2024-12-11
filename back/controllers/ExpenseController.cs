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
        private readonly IAuthService _authService;

        public ExpenseController(ApiDbContext context, IAuthService authService)
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
        public async Task<ActionResult<IEnumerable<ExpenseDTO>>> Get()
        {
            try
            {
                var username = await _authService.ValidateTokenFromRequest(Request);
                if (username == null)
                    return Unauthorized(new { message = "Invalid token" });

                var expenses = await _context.Expenses
                    .Where(e => e.User.username == username)
                    .Select(e => new ExpenseDTO
                    {
                        Id = e.Id,
                        Amount = e.Amount,
                        Date = e.Date,
                        Category = e.Category
                    })
                    .OrderByDescending(e => e.Date)
                    .ToListAsync();

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
        [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExpenseDTO expenseDto)
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

            var expense = new Expenses
            {
                Amount = expenseDto.Amount,
                Date = expenseDto.Date,
                Category = expenseDto.Category,
                UserId = user.Id
            };

            var oldAmount = user.Budget.CurrentAmount;
            user.Budget.CurrentAmount += expense.Amount;
            _context.Entry(user.Budget).State = EntityState.Modified;

            Console.WriteLine($"New budget after update: {user.Budget.CurrentAmount}");

            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();

            return Ok(new { 
                message = "Expense added successfully",
                oldBudget = oldAmount,
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var username = await _authService.ValidateTokenFromRequest(Request);
                if (username == null)
                    return Unauthorized(new { message = "Invalid token" });

                var expense = await _context.Expenses
                    .Include(e => e.User)
                    .Include(e => e.User.Budget)
                    .FirstOrDefaultAsync(e => e.Id == id && e.User.username == username);

                if (expense == null)
                    return NotFound(new { message = "Expense not found" });

                if (expense.User.Budget != null)
                {
                    expense.User.Budget.CurrentAmount -= expense.Amount;
                    _context.Entry(expense.User.Budget).State = EntityState.Modified;
                }
                _context.Entry(expense.User.Budget).State = EntityState.Modified;

                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();

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