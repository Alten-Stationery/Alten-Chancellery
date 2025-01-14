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
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<ItemDTO>> Add(ItemDTO dTO)
        {
            try
            {
                var item = _mapper.Map<Item>(dTO);
                var res = await _unitOfWork.itemRepository.CreateAsync(item);
                if (res is null) return new Response<ItemDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = "Error Saving the item" };
                var itemToSend = _mapper.Map<ItemDTO>(res);
                return new Response<ItemDTO> { StatusCode = System.Net.HttpStatusCode.OK, Data = itemToSend };
            }
            catch (Exception ex)
            {
                return new Response<ItemDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }

        public async Task<Response<List<ItemDTO>>> GetAll()
        {
            try
            {
                var itemList = await _unitOfWork.itemRepository.GetAllAsync();
                if (itemList is null) return new Response<List<ItemDTO>> { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "Error: No ItemFound" };
                var itemListDTO = _mapper.Map<List<ItemDTO>>(itemList);
                return new Response<List<ItemDTO>> { StatusCode = System.Net.HttpStatusCode.OK, Data = itemListDTO };
            }
            catch (Exception ex)
            {
                return new Response<List<ItemDTO>> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }

        }

        public async Task<Response<ItemDTO>> GetById(int id)
        {
            try
            {
                var item = await _unitOfWork.itemRepository.FindAsync(id);
                if (item is null) return new Response<ItemDTO> { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "Item not Found" };
                var itemDTO = _mapper.Map<ItemDTO>(item);
                return new Response<ItemDTO> { StatusCode = System.Net.HttpStatusCode.OK, Data = itemDTO };
            }
            catch (Exception ex)
            {
                return new Response<ItemDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }



        public async Task<Response<bool>> Remove(int id)
        {
            try
            {
                var itemToDelete = await _unitOfWork.OfficeRepository.FindAsync(id);
                if (itemToDelete is null) return new Response<bool> { StatusCode = System.Net.HttpStatusCode.NotFound, Data = false, Message = "Error: Item not found" };
                var res = await _unitOfWork.OfficeRepository.DeleteAsync(itemToDelete);
                return new Response<bool> { StatusCode = System.Net.HttpStatusCode.OK, Data = true };
            }
            catch (Exception ex)
            {
                return new Response<bool> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }

        public async Task<Response<bool>> Update(ItemDTO itemDTO)
        {
            try
            {
                var item = _mapper.Map<Item>(itemDTO);
                var res = await _unitOfWork.itemRepository.UpdateAsync(item);
                if (!res) return new Response<bool> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Data = false };
                return new Response<bool> { StatusCode = System.Net.HttpStatusCode.OK, Data = true };
            }
            catch (Exception ex)
            {
                return new Response<bool> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }
    }
}
