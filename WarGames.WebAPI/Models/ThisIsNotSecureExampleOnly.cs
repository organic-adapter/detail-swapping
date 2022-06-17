namespace WarGames.WebAPI.Models
{
	public class ThisIsNotSecureExampleOnly
	{
		public ThisIsNotSecureExampleOnly()
		{
			Users = new Dictionary<string, string>();
		}

		public JwtDetails Jwt { get; set; }
		public Dictionary<string, string> Users { get; set; }
		public class JwtDetails
		{
			public string Key { get; set; } = string.Empty;
			public string Subject { get; set; } = string.Empty;
			public string Issuer { get; set; } = string.Empty;
			public string Audience { get; set; } = string.Empty;

		}
	}
}