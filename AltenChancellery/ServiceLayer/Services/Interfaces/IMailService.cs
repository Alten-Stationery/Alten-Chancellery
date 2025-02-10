using DBLayer.Models;
using ServiceLayer.DTOs.Common;

namespace ServiceLayer.Services.Interfaces
{
	public interface IMailService
	{
		Task<Response<bool>> SendEmail(MailData mailData);
	}
}