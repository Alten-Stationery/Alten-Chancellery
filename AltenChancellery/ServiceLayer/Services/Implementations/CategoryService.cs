using AutoMapper;
using DBLayer.Models;
using DBLayer.UnitOfWork;
using ServiceLayer.DTOs;
using ServiceLayer.DTOs.Common;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<CategoryDTO>> Add(CategoryDTO dto)
        {
            bool operationResult = Enum.TryParse(typeof(CategoryName), dto.Name, out var convertedValue);

            if (!operationResult)
                throw new InvalidCastException("Category name doesnt exists");

            Category? alreadyExists = _unitOfWork.CategoryRepository.GetAllAsync().Result
                .FirstOrDefault(f => f.Name == (CategoryName)convertedValue!);

            if (alreadyExists != null)
                throw new Exception("Category already exists");

            Category input = _unitOfWork.CategoryRepository.Create(new Category() { Name = (CategoryName)convertedValue! });

            await _unitOfWork.SaveAsync();

            return new Response<CategoryDTO>()
            {
                Data = _mapper.Map<CategoryDTO>(input),
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response<List<CategoryDTO>>> GetAll()
        {
            IList<Category> output = await _unitOfWork.CategoryRepository.GetAllAsync();

            return new Response<List<CategoryDTO>>()
            {
                Data = _mapper.Map<List<CategoryDTO>>(output),
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response<CategoryDTO>> GetById(int id)
        {
            Category? result = await _unitOfWork.CategoryRepository.FindAsync(id);

            return new Response<CategoryDTO>() { Data = _mapper.Map<CategoryDTO>(result), StatusCode = System.Net.HttpStatusCode.OK };
        }

        public async Task<Response<bool>> Remove(int id)
        {
            Category? result = await _unitOfWork.CategoryRepository.FindAsync(id);

            if (result != null)
            {
                bool operationResult = _unitOfWork.CategoryRepository.Delete(result);

                await _unitOfWork.SaveAsync();

                return new Response<bool>()
                {
                    Data = operationResult,
                    StatusCode = !operationResult ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.InternalServerError
                };
            }

            return new Response<bool>() { Data = false, StatusCode = System.Net.HttpStatusCode.NotFound };
        }

        public async Task<Response<bool>> RemoveAll()
        {
            IList<Category> categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            bool resultFlag = true;

            foreach (Category category in categories)
            {
                try
                {
                    _unitOfWork.CategoryRepository.Delete(category);
                }
                catch (Exception) { resultFlag = false; break; }
            }

            return new Response<bool>()
            {
                Data = resultFlag,
                StatusCode = resultFlag ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.InternalServerError
            };
        }

        public Task<Response<bool>> Update(CategoryDTO itemDTO)
        {
            throw new NotImplementedException();
        }
    }
}
