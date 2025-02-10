
namespace DBLayer.Models
{
	public class MailData
	{
		public List<string> EmailToAdresses { get; set; }
		public string EmailSubject { get; set; }
		public string EmailBody { get; set; }

		public MailData()
		{
			EmailToAdresses = new List<string>();
		}
	}
}