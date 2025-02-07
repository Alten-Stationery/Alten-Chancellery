using AutoMapper;
using DBLayer.Models;
using DBLayer.UnitOfWork;
using ServiceLayer.DTOs;
using ServiceLayer.DTOs.Common;
using ServiceLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                if (DateTime.Now <= dTO.Date) return new Response<AlertDTO> { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Error: Bad Date for this Alert" };
                
                var alert = _mapper.Map<Alert>(dTO);
                
                var rlsEmail = await _unitOfWork.itemOfficeRepository.GetRLSEmail(dTO.OfficeId);
                
                if (rlsEmail is null) return new Response<AlertDTO> { StatusCode = HttpStatusCode.InternalServerError, Message = "No RLS User Found for this Office" };
                //var isEmailSend = SendEmailAlert(rlsEmail);
                
                var res = _unitOfWork.AlertRepository.Create(alert);
                
                await _unitOfWork.SaveAsync();
                
                if (res is null) return new Response<AlertDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = "Error: Error during saving entity" };
                
                var alertDTO = _mapper.Map<AlertDTO>(res);
                
                return new Response<AlertDTO> { StatusCode = System.Net.HttpStatusCode.Accepted, Data = alertDTO };
            }
            catch(Exception ex)
            {
                return new Response<AlertDTO>() { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }

        }

        
        public async Task<Response<List<AlertDTO>>> GetAll()
        {
            try
            {
                var ListAlert = await _unitOfWork.AlertRepository.GetAllAsync();
                if(ListAlert.Count() == 0) return new Response<List<AlertDTO>>() { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "No Alert Found"};
                var listAlertDTO = _mapper.Map<List<AlertDTO>>(ListAlert);
                return new Response<List<AlertDTO>>() { StatusCode = System.Net.HttpStatusCode.OK , Data = listAlertDTO };

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
                var alert = await _unitOfWork.AlertRepository.FindAsync(id);
                if (alert is null) return new Response<AlertDTO>() { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "Not Found" };
                var alertDTO = _mapper.Map<AlertDTO>(alert);
                return new Response<AlertDTO>() { StatusCode = System.Net.HttpStatusCode.OK, Data = alertDTO };
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
                var alert = await _unitOfWork.AlertRepository.FindAsync(id);
                if (alert is null) return new Response<bool> { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "AlertNotFound" };
                var res =  _unitOfWork.AlertRepository.Delete(alert);
                await _unitOfWork.SaveAsync();
                if (!res) return new Response<bool> {StatusCode = System.Net.HttpStatusCode.InternalServerError, Data = res };
                return new Response<bool>() {StatusCode = System.Net.HttpStatusCode.OK, Data = res };
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
                var alert = await _unitOfWork.AlertRepository.FindAsync(itemDTO.AlertId);
                var res = _unitOfWork.AlertRepository.Update(alert);
                await _unitOfWork.SaveAsync();
                if (!res) return new Response<bool> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = "Error Updating the alert"};
                return new Response<bool>() { StatusCode = System.Net.HttpStatusCode.OK, Data = res };  
            }
            catch (Exception ex)
            {
                return new Response<bool>() { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }
    }
}
