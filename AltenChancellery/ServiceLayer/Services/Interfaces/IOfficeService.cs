using ServiceLayer.DTOs;
using ServiceLayer.DTOs.Common;

namespace ServiceLayer.Services.Interfaces
{
    public interface IOfficeService
    {
        Task<Response<OfficeDTO>> AddOfficeAsync(OfficeDTO officeDTO);
        Task<Response<bool>> RemoveOfficeAsync(string officeId);
        Task<Response<OfficeDTO>> GetOfficeByIdAsync(string id);
        Task<Response<List<OfficeDTO>>> GetAllOfficesAsync();
        Task<Response<OfficeDTO>> UpdateOfficeAsync(OfficeDTO officeDTO);
    }
}
