using AutoMapper;
using DBLayer.Models;
using DBLayer.UnitOfWork;
using ServiceLayer.DTOs;
using ServiceLayer.DTOs.Common;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork uow, IMapper mapper)
        {
            _unitOfWork = uow;
            _mapper = mapper;
        }

        public async Task<Response<NotificationDTO>> Add(NotificationDTO dto)
        {
            Notification input = _mapper.Map<Notification>(dto);

            Notification result = _unitOfWork.NotificationRepository.Create(input);

            return new Response<NotificationDTO>() { Data = _mapper.Map<NotificationDTO>(result), StatusCode = System.Net.HttpStatusCode.OK };
        }

        public async Task<Response<List<NotificationDTO>>> GetAll()
        {
            IList<Notification> output = await _unitOfWork.NotificationRepository.GetAllAsync();

            return new Response<List<NotificationDTO>>()
            {
                Data = _mapper.Map<List<NotificationDTO>>(output),
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response<NotificationDTO>> GetById(int id)
        {
            Notification? result = await _unitOfWork.NotificationRepository.FindAsync(id);

            return new Response<NotificationDTO>() { Data = _mapper.Map<NotificationDTO>(result), StatusCode = System.Net.HttpStatusCode.OK };
        }

        public async Task<Response<bool>> Remove(int id)
        {
            Notification? result = await _unitOfWork.NotificationRepository.FindAsync(id);

            if (result != null)
            {
                bool operationResult = _unitOfWork.NotificationRepository.Delete(result);
                return new Response<bool>()
                {
                    Data = operationResult,
                    StatusCode = !operationResult ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.InternalServerError
                };
            }

            return new Response<bool>() { Data = false, StatusCode = System.Net.HttpStatusCode.NotFound };
        }

        public Task<Response<bool>> Update(NotificationDTO itemDTO)
        {
            throw new NotImplementedException();
        }
    }
}
