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
            var res = await _officeService.AddOffice(officeDTO);
            return Ok(res);
        }


        [HttpGet]
        [Route("GetOfficebyId")]
        public async Task<IActionResult> GetOfficeById(string officeId)
        {
            var res = await _officeService.GetOfficeById(officeId);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _officeService.GetAllOffices();
            return Ok(res);
        }

        [HttpPut]
        [Route("UpdateOffice")]
        public async Task<IActionResult> Update(OfficeDTO officeDTO)
        { 
            var res = await _officeService.UpdateOffice(officeDTO);
            return Ok(res);
        }

        [HttpDelete]
        [Route("DeleteOffice")]
        public async Task<IActionResult> DeleteOffice(string officeId)
        { 
            var res = await _officeService.RemoveOffice(officeId);
            return Ok(res);
        }
    }
}
