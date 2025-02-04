using DBLayer.DBContext;
using DBLayer.Models;
using DBLayer.Repositories.Interfaces;

namespace DBLayer.Repositories.Implementations
{
    public class CategoryRepository : GenericRepository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDBContext context) : base(context)
        {
            Array enumValues = Enum.GetValues(typeof(CategoryName));

            bool isEnumFullyCreated = GetAllAsync().Result.Count() == enumValues.Length;

            if (!isEnumFullyCreated)
            {
                foreach (int item in enumValues)
                {
                    Category? category = FindByName(((CategoryName)item).ToString());

                    // checks foreach enum value if exists
                    if (category == null)
                    {
                        _dbSet.Add(new Category()
                        {
                            Name = (CategoryName)item
                        });

                        SaveChanges();
                    }
                }
            }
        }

        public Category? FindByName(string name) => _dbSet.FirstOrDefault(f => f.Name.ToString() == name);
    }
}
