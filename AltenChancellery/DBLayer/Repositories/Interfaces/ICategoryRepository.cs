using DBLayer.Models;

namespace DBLayer.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category, int>
    {
        Category? FindByName(string name);
    }
}
