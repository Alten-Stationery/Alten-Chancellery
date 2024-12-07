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
        Task<Response<OfficeDTO>> AddOffice(OfficeDTO officeDTO);
        Task<Response<bool>> RemoveOffice(string officeId);
        Task<Response<OfficeDTO>> GetOfficeById(string id);
        Task<Response<List<OfficeDTO>>> GetAllOffices();
        Task<Response<OfficeDTO>> UpdateOffice(OfficeDTO officeDTO);
    }
}
