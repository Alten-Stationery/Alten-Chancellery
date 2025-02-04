using DBLayer.Models;

namespace ServiceLayer.DTOs
{
    public class NotificationDTO
    {
        public int CategoryId { get; set; }
        public string UserEmail { get; set; } = null!;
    }
}
