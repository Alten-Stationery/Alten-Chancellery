using ServiceLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface GenericServices<T, TId> where T : class
    {
        Task<Response<T>> Add(T dTO);
        Task<Response<bool>> Remove(TId id);
        Task<Response<T>> GetById(TId id);
        Task<Response<List<T>>> GetAll();
        Task<Response<bool>> Update(T itemDTO);
    }
}
