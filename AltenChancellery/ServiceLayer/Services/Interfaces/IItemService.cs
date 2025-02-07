using ServiceLayer.DTOs;
using ServiceLayer.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface IItemService 
    {
        Task<Response<ItemDTO>> Add(ItemDTO dto);
        Task<Response<bool>> Remove(int id);
        Task<Response<ItemDTO>> GetById(int id);
        Task<Response<List<ItemDTO>>> GetAll();
        Task<Response<bool>> Update(ItemDTO dto);
    }
}
