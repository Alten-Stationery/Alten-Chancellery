using AutoMapper;
using DBLayer.Models;
using DBLayer.UnitOfWork;
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
    public class AlertService : IAlertService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AlertService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<Response<AlertDTO>> Add(AlertDTO dTO)
        {
            try 
            {
                if (!CheckDate(dTO.Date)) return new Response<AlertDTO> { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Error: Bad Date for this Alert" };
                var alert = _mapper.Map<Alert>(dTO);
                var res = _unitOfWork.AlertRepository.Create(alert);
                if(res is  null) return new Response<AlertDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = "Error: Error during saving entity"
                
            }
            catch(Exception ex)
            {
                return new Response<AlertDTO>() { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }

        }
        private bool CheckDate(DateTime date) 
        {
            if (DateTime.Now <= date) return false;

            return true;

        }
        public async Task<Response<List<AlertDTO>>> GetAll()
        {
            try
            {
            }
            catch (Exception ex)
            {
                return new Response<List<AlertDTO>>() { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }

        public async Task<Response<AlertDTO>> GetById(int id)
        {
            try
            {
            }
            catch (Exception ex)
            {
                return new Response<AlertDTO>() { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }

        public async Task<Response<bool>> Remove(int id)
        {
            try
            {
            }
            catch (Exception ex)
            {
                return new Response<bool>() { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }

        public async Task<Response<bool>> Update(AlertDTO itemDTO)
        {
            try
            {
            }
            catch (Exception ex)
            {
                return new Response<bool>() { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }
    }
}
