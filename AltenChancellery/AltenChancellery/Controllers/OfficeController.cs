using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs;
using ServiceLayer.Services.Interfaces;

namespace AltenChancellery.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class OfficeController : ControllerBase
    {
        private readonly IOfficeService _officeService;
        public OfficeController(IOfficeService officeService)
        {
            _officeService = officeService;

        }

        [HttpPost]
        [Route("CreateOffice")]
        public async Task<IActionResult> CreateOffice(OfficeDTO officeDTO)
        {
            var res = await _officeService.Add(officeDTO);
            return Ok(res);
        }


        [HttpGet]
        [Route("GetOfficebyId")]
        public async Task<IActionResult> GetOfficeById(int officeId)
        {
            var res = await _officeService.GetById(officeId);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _officeService.GetAll();
            return Ok(res);
        }

        [HttpPut]
        [Route("UpdateOffice")]
        public async Task<IActionResult> Update(OfficeDTO officeDTO)
        { 
            var res = await _officeService.Update(officeDTO);
            return Ok(res);
        }

        [HttpDelete]
        [Route("DeleteOffice")]
        public async Task<IActionResult> DeleteOffice(int officeId)
        { 
            var res = await _officeService.Remove(officeId);
            return Ok(res);
        }
    }
}
