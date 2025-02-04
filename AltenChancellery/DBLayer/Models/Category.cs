namespace DBLayer.Models
{
    public class Category
    {
        public int Id { get; set; }
        public CategoryName Name { get; set; }
        public virtual IList<Notification> Notifications { get; set; }
    }

    // Those catecories are automatically saved in Database, just add the new name
    // |------>  See CategoryRepository constructor
    public enum CategoryName
    {
        Chancellery,
        Medicine,
        Antifire,
        Water
    }
}
