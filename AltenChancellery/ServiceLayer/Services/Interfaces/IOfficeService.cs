using ServiceLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
