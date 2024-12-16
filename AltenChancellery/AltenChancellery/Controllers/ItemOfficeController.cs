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
        [Route("Add")]
        public async Task<IActionResult> AddItem(ItemOfficeDTO itemDTO)
        {
            var res = await _itemOfficeService.AddItemOffice(itemDTO);
            return Ok(res);

        }
        [HttpPost]
        [Route("Find")]
        public async Task<IActionResult> Find(int itemId, int officeId)
        {
            var res = await _itemOfficeService.GetItemOfficeById(itemId, officeId);
            return Ok(res);
        }
        [HttpPost]
        [Route("FindAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _itemOfficeService.GetAllItemOffices();
            return Ok(res);

        }
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(ItemOfficeDTO itemDTO)
        {
            var res = await _itemOfficeService.UpdateItemOffice(itemDTO);
            return Ok(res);

        }
        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> Remove(int itemId, int officeId)
        {
            var res = await _itemOfficeService.RemoveItemOffice(itemId, officeId);
            return Ok(res);

        }
    }
}
