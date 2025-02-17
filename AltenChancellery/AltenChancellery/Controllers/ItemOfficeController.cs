using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs;
using ServiceLayer.Services.Interfaces;

namespace AltenChancellery.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ItemOfficeController : ControllerBase
    {
        private readonly IItemOfficeService _itemOfficeService;
        public ItemOfficeController(IItemOfficeService itemOfficeService)
        {
            _itemOfficeService = itemOfficeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(ItemOfficeDTO itemDTO)
        {
            var res = await _itemOfficeService.AddItemOffice(itemDTO);
            return Ok(res);

        }
        [HttpGet]
        public async Task<IActionResult> Find([FromQuery]int itemId, [FromQuery]int officeId)
        {
            var res = await _itemOfficeService.GetItemOfficeById(itemId, officeId);
            return Ok(res);
        }
        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _itemOfficeService.GetAllItemOffices();
            return Ok(res);

        }
        [HttpPut]
        public async Task<IActionResult> Update(ItemOfficeDTO itemDTO)
        {
            var res = await _itemOfficeService.UpdateItemOffice(itemDTO);
            return Ok(res);

        }
        [HttpDelete]
        public async Task<IActionResult> Remove([FromQuery] int itemId, [FromQuery] int officeId)
        {
            var res = await _itemOfficeService.RemoveItemOffice(itemId, officeId);
            return Ok(res);

        }
    }
}
