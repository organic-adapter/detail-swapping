using WarGames.Contracts.Game;

namespace WarGames.Business.Game
{
	public class Player : IPlayer
	{
		public Player(string name, string id, PlayerType playerType)
		{
			Name = name;
			Id = id;
			PlayerType = playerType;
		}

		public Player(string name, string id)
		{
			Name = name;
			Id = id;
		}

		public Player()
		{
			Name = string.Empty;
			Id = string.Empty;
		}

		public string Id { get; set; }
		public string Name { get; set; }
		public PlayerType PlayerType { get; set; }
	}
}