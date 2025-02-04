using ServiceLayer.DTOs;
using ServiceLayer.DTOs.Common;

namespace ServiceLayer.Services.Interfaces
{
    public interface ICategoryService : GenericServices<CategoryDTO, int>
    {
        Task<Response<bool>> RemoveAll();
    }
}
