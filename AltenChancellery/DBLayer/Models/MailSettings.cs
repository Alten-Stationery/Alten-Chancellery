
namespace DBLayer.Models
{
	public class MailSettings
	{
		public string Server { get; set; }
		public int Port { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public bool UseSSL { get; set; }
	}
}