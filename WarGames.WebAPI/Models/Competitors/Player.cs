using WarGames.Contracts.Game;

namespace WarGames.WebAPI.Models.Competitors
{
	public class Player : IPlayer
	{
		public string Id { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
	}
}