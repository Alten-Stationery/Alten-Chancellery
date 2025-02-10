using DBLayer.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services.Interfaces;

namespace AltenChancellery.Controllers
{
	[ApiController]
	[Route("api/[Controller]")]
	public class MailController : ControllerBase
	{
		private readonly IMailService _mailService;
		public MailController(IMailService mailService)
		{
			_mailService = mailService;
		}

		[HttpPost]
		[Route("SendMail")]
		public async Task<IActionResult> SendMail(MailData mailData)
		{
			var res = await _mailService.SendEmail(mailData);
			return Ok(res);
		}
	}
}