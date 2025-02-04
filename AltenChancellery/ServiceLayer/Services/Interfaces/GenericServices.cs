using ServiceLayer.DTOs;
using ServiceLayer.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface GenericServices<T, TID> where T : class
    {
        Task<Response<T>> Add(T dto);
        Task<Response<bool>> Remove(TID id);
        Task<Response<T>> GetById(TID id);
        Task<Response<List<T>>> GetAll();
        Task<Response<bool>> Update(T itemDTO);
    }
}
