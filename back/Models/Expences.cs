namespace MyFirstApi.Models
{
    public class Expenses
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; } = string.Empty;

        public int UserId { get; set; }
        public Users User { get; set; } = null!;
        
    }
}