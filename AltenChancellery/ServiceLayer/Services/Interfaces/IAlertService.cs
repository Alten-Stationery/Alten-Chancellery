using DBLayer.Repositories.Interfaces;
using ServiceLayer.DTOs.Common;
using ServiceLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface IAlertService 
    {
        Task<Response<AlertDTO>> Add(AlertDTO dto);
        Task<Response<bool>> Remove(int id);
        Task<Response<AlertDTO>> GetById(int id);
        Task<Response<List<AlertDTO>>> GetAll();
        Task<Response<bool>> Update(AlertDTO dto);
    }
}
