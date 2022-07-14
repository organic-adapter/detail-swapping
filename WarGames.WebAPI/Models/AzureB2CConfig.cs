namespace WarGames.WebAPI.Models
{
	public class AzureB2CConfig
	{
		public AzureB2CConfig()
		{
			Authority = string.Empty;
			ClientId = string.Empty;
		}

		public string Authority { get; set; }
		public string ClientId { get; set; }
	}
}