namespace WarGames.WebAPI.Models
{
	public class IdentityProviderDetails
	{
		public IdentityProviderDetails()
		{
			Details = new ();
		}
		public Dictionary<string, string> Details { get; set; } 
	}
}
