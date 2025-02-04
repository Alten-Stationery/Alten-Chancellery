namespace DBLayer.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string UserEmail { get; set; } = null!;
        public virtual IList<User> Users { get; set; }
    }
}
