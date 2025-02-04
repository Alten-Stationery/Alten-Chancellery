using DBLayer.DBContext;
using DBLayer.Models;
using DBLayer.Repositories.Interfaces;

namespace DBLayer.Repositories.Implementations
{
    public class NotificationRepository : GenericRepository<Notification, int>, INotificationRepository
    {
        public NotificationRepository(ApplicationDBContext context) : base(context)
        {
        }
    }
}
