namespace MyFirstApi.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal MaxAmount { get; set; }
        
        public int UserId { get; set; }
        public virtual Users User { get; set; } = null!;
    }
}