using ServiceLayer.DTOs;
using ServiceLayer.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface IItemOfficeService 
    {
        Task<Response<ItemOfficeDTO>> AddItemOffice(ItemOfficeDTO officeDTO);
        Task<Response<bool>> RemoveItemOffice(int itemId, int officeid );
        Task<Response<ItemOfficeDTO>> GetItemOfficeById(int itemId, int officeId);
        Task<Response<List<ItemDTO>>> GetAllTheItembyOffice(int officeId);
        Task<Response<List<ItemOfficeDTO>>> GetAllItemOffices();
        Task<Response<bool>> UpdateItemOffice(ItemOfficeDTO itemOfficeDTO);
    }
}
