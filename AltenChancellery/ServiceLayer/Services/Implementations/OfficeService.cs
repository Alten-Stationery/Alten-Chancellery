using AutoMapper;
using DBLayer.Models;
using DBLayer.Repositories.Interfaces;
using DBLayer.UnitOfWork;
using Microsoft.Identity.Client;
using ServiceLayer.DTOs;
using ServiceLayer.DTOs.Common;
using ServiceLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Implementations
{
    public class OfficeService : IOfficeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;  
        public OfficeService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            
        }
        public async Task<Response<OfficeDTO>> Add(OfficeDTO officeDTO)
        {
            try 
            {
                var office = _mapper.Map<Office>(officeDTO);
                var res = _unitOfWork.OfficeRepository.Create(office);
                if (res == null) return new Response<OfficeDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = "Error saving the Office" };
                var officeToSend = _mapper.Map<OfficeDTO>(res);

                await _unitOfWork.SaveAsync();

                return new Response<OfficeDTO> {StatusCode = System.Net.HttpStatusCode.OK, Data = officeToSend };
            }
            catch(Exception ex) 
            {
                return new Response<OfficeDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }

        public async Task<Response<List<OfficeDTO>>> GetAll()
        {
            try
            {
                var officeList = await _unitOfWork.OfficeRepository.GetAllAsync();
                if (officeList is null) return new Response<List<OfficeDTO>> { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "No Office Found" };
                var officeListDTO = _mapper.Map<List<OfficeDTO>>(officeList);
                return new Response<List<OfficeDTO>> { StatusCode = System.Net.HttpStatusCode.OK, Data=officeListDTO };
            }
            catch (Exception ex)
            {
                return new Response<List<OfficeDTO>> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }

        public async Task<Response<OfficeDTO>> GetById(int id)
        {
            try
            {
                var office = await _unitOfWork.OfficeRepository.FindAsync(id);
                if (office is null) return new Response<OfficeDTO> { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "No Office Found" };
                var officeDTO = _mapper.Map<OfficeDTO>(office);
                return new Response<OfficeDTO> { StatusCode = System.Net.HttpStatusCode.OK, Data = officeDTO };
            }
            catch (Exception ex)
            {
                return new Response<OfficeDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }

        public async Task<Response<bool>> Remove(int officeId)
        {
            try
            {
                var office = _unitOfWork.OfficeRepository.Find(officeId);
                if (office is null) { return new Response<bool> {StatusCode = System.Net.HttpStatusCode.NotFound, Data = false}; }

                bool isDeleted = _unitOfWork.OfficeRepository.Delete(office);
                if(!isDeleted) { return new Response<bool> { StatusCode = System.Net.HttpStatusCode.NotFound, Data = false }; }

                await _unitOfWork.SaveAsync();

                var officeDTO = _mapper.Map<OfficeDTO>(office);
                return new Response<bool> { StatusCode = System.Net.HttpStatusCode.OK, Data = true };
            }
            catch (Exception ex)
            {
                return new Response<bool> { StatusCode=System.Net.HttpStatusCode.InternalServerError,Data = false, Message = ex.Message};
            }
        }

        public async Task<Response<bool>> Update(OfficeDTO officeDTO)
        {
            try
            {
                var office = _mapper.Map<Office>(officeDTO);
                var res = await _unitOfWork.OfficeRepository.UpdateAsync(office);
                if (!res) return new Response<bool> { StatusCode = System.Net.HttpStatusCode.InternalServerError,Data = false, Message = "Error during Updating the office" };
                var officeDTOToSend = _mapper.Map<OfficeDTO>(res);
                return new Response<bool> { StatusCode = System.Net.HttpStatusCode.Accepted, Data = true };
            }
            catch (Exception ex)
            {
                return new Response<bool> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            } 
        }
    }
}
