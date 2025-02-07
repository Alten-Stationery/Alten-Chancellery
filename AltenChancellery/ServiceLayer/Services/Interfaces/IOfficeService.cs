using ServiceLayer.DTOs;
using ServiceLayer.DTOs.Common;
using System.Security.Cryptography;

namespace ServiceLayer.Services.Interfaces
{
    public interface IOfficeService 
    {
        Task<Response<OfficeDTO>> Add(OfficeDTO dto);
        Task<Response<bool>> Remove(int id);
        Task<Response<OfficeDTO>> GetById(int id);
        Task<Response<List<OfficeDTO>>> GetAll();
        Task<Response<bool>> Update(OfficeDTO dto);
    }
}
