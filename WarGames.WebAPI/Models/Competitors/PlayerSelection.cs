namespace WarGames.WebAPI.Models.Competitors
{
	public class PlayerSelection
	{
		public string CompetitorName { get; set; } = string.Empty;
		public Player? Player { get; set; }
		public string WorldId { get; set; } = string.Empty;
	}
}