
using AutoMapper;
using DBLayer.Models;
using DBLayer.UnitOfWork;
using ServiceLayer.DTOs;
using ServiceLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Implementations
{
    public class ItemOfficeService : IItemOfficeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ItemOfficeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<ItemOfficeDTO>> AddItemOffice(ItemOfficeDTO itemOfficeDTO)
        {
            var itemOffice = _mapper.Map<ItemOffice>(itemOfficeDTO);
            var res = await _unitOfWork.itemOfficeRepository.CreateAsync(itemOffice);
            if (res is null) return new Response<ItemOfficeDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = "Error: Error saving itemoffice" };
            var itemOfficeToSend = _mapper.Map<ItemOfficeDTO>(res);
            return new Response<ItemOfficeDTO> { StatusCode = System.Net.HttpStatusCode.OK, Data = itemOfficeToSend };
        }

        public async Task<Response<List<ItemOfficeDTO>>> GetAllItemOffices()
        {
            var officeList = await _unitOfWork.itemOfficeRepository.GetAllAsync();
            if (officeList.Count == 0) return new Response<List<ItemOfficeDTO>> { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Error: ItemOffice Not Found" };
            var officeListDTO = _mapper.Map<List<ItemOfficeDTO>>(officeList);
            return new Response<List<ItemOfficeDTO>> { StatusCode = System.Net.HttpStatusCode.OK, Data = officeListDTO };
        }


        public async Task<Response<List<ItemDTO>>> GetAllTheItembyOffice(int officeId)
        {
            try
            {
                var itemList = await _unitOfWork.itemOfficeRepository.GetItemFromOffice(officeId);
                if (itemList.Count() == 0) return new Response<List<ItemDTO>> { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "ERROR, no Item Found" };
                var itemDTOList = _mapper.Map<List<ItemDTO>>(itemList);
                return new Response<List<ItemDTO>> { StatusCode = System.Net.HttpStatusCode.OK, Data = itemDTOList, Message = "Mapper" };

            }
            catch (Exception ex)
            {
                return new Response<List<ItemDTO>> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }

        public async Task<Response<ItemOfficeDTO>> GetItemOfficeById(int itemId, int officeId)
        {
            try 
            {
                var itemOffice = await _unitOfWork.itemOfficeRepository.GetItemOfficeById(officeId, itemId);
                if (itemOffice == null) return new Response<ItemOfficeDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = "Error: No ItemOffice Found" };
                var itemOfficeDTO = _mapper.Map<ItemOfficeDTO>(itemOffice);
                return new Response<ItemOfficeDTO> { StatusCode = System.Net.HttpStatusCode.OK, Data = itemOfficeDTO };
            }
            catch(Exception ex) 
            {
                return new Response<ItemOfficeDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }

        public async Task<Response<bool>> RemoveItemOffice(int itemId, int officeId)
        {
            try
            {
                var ItemOfficeToRemove = await _unitOfWork.itemOfficeRepository.GetItemOfficeById(officeId, itemId);
                var res = await _unitOfWork.itemOfficeRepository.DeleteAsync(ItemOfficeToRemove);
                if (!res) return new Response<bool> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = "Error deleting the itemOffice" };
                return new Response<bool> { StatusCode = System.Net.HttpStatusCode.OK, Data = res };
            }
            catch (Exception ex)
            {
                return new Response<bool> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Data = false, Message = ex.Message };
            }
        }

        public async Task<Response<bool>> UpdateItemOffice(ItemOfficeDTO itemOfficeDTO)
        {
            try
            {
                var itemOffice = _mapper.Map<ItemOffice>(itemOfficeDTO);
                var res = await _unitOfWork.itemOfficeRepository.UpdateAsync(itemOffice);
                if (!res) return new Response<bool> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Data = res,  Message = "Error updating ItemOffice" };
                return new Response<bool> { StatusCode = System.Net.HttpStatusCode.OK, Data = res };

            }
            catch (Exception ex)
            {
                return new Response<bool> { StatusCode = System.Net.HttpStatusCode.OK, Message = ex.Message };

            }
        }
    }
}
