using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs;
using ServiceLayer.Services.Interfaces;

namespace AltenChancellery.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AlertController : ControllerBase
    {
        private readonly IAlertService _alertService;
        public AlertController(IAlertService alertService)
        {
            _alertService = alertService;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int alertId)
        {
            var res = await _alertService.GetById(alertId);
            return Ok(res);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(AlertDTO alertDTO)
        {
            var res = await _alertService.Add(alertDTO);
            return Ok(res);

        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        { 
            var res = await _alertService.GetAll();
            return Ok(res);

        }
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(AlertDTO alertDTO)
        {
            var res = await _alertService.Update(alertDTO);
            return Ok(res);
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove(int id)
        {
            var res = await _alertService.Remove(id);
            return Ok(res);
        }

    }
}
