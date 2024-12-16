using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs;
using ServiceLayer.Services.Interfaces;

namespace AltenChancellery.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService itemService) 
        {
            _itemService = itemService;
        }


        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddItem(ItemDTO itemDTO )
        {
            var res = await _itemService.Add(itemDTO);
            return Ok(res);
        
        }
        [HttpPost]
        [Route("Find")]
        public async Task<IActionResult> Find(int itemId)
        {
            var res = await _itemService.GetById(itemId);
            return Ok(res);
        }
        [HttpPost]
        [Route("FindAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _itemService.GetAll();
            return Ok(res);

        }
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(ItemDTO itemDTO)
        {
            var res = await _itemService.Update(itemDTO);
            return Ok(res);

        }
        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> Remove(int itemId)
        {
            var res = await _itemService.Remove(itemId);
            return Ok(res);

        }


    }
}
