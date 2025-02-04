using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs;
using ServiceLayer.Services.Interfaces;

namespace AltenChancellery.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("Find")]
        public async Task<IActionResult> Find(int itemId)
        {
            var res = await _categoryService.GetById(itemId);
            return Ok(res);
        }

        [HttpGet]
        [Route("FindAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _categoryService.GetAll();
            return Ok(res);

        }
        
        [HttpDelete]
        [Route("RemoveAll")]
        public async Task<IActionResult> RemoveAll()
        {
            var res = await _categoryService.RemoveAll();
            return Ok(res);

        }
    }
}
