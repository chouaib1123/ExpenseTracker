namespace MyFirstApi.Models
{
    public class Users
    {
        public Users()
        {
            Expenses = new List<Expenses>();
        }

        public int Id { get; set; }
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;

        public virtual ICollection<Expenses> Expenses { get; set; }

        public virtual Budget? Budget { get; set; }
    }
}