using DBLayer.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using ServiceLayer.DTOs.Common;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Services.Implementations
{
	public class MailService : IMailService
	{
		private readonly MailSettings _mailSettings;
		public MailService(IOptions<MailSettings> mailSettingOptions)
		{
			_mailSettings = mailSettingOptions.Value;
		}

		public async Task<Response<bool>> SendEmail(MailData mailData)
		{
			try
			{

				MimeMessage email_Message = new MimeMessage();
				MailboxAddress email_From = new MailboxAddress("Alten Chancellery", "noreplay@notificationservice.it");
				email_Message.From.Add(email_From);
				MailboxAddress email_To = new MailboxAddress(string.Join(";", mailData.EmailToAdresses), string.Join(";", mailData.EmailToAdresses));
				email_Message.To.Add(email_To);
				email_Message.Subject = mailData.EmailSubject;
				BodyBuilder emailBodyBuilder = new BodyBuilder();
				emailBodyBuilder.TextBody = mailData.EmailBody;
				email_Message.Body = emailBodyBuilder.ToMessageBody();

				SmtpClient MailClient = new SmtpClient();
				MailClient.Connect(_mailSettings.Server, _mailSettings.Port, _mailSettings.UseSSL);
				MailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
				await MailClient.SendAsync(email_Message);
				MailClient.Disconnect(true);
				MailClient.Dispose();
				return new Response<bool> { StatusCode = System.Net.HttpStatusCode.OK, Data = true };
			}
			catch (Exception ex)
			{
				// Exception Details
				return new Response<bool> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Data = false, Message = ex.Message };
			}
		}
	}
}