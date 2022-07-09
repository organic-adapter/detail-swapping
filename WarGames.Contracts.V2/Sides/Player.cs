using WarGames.Contracts.Game;

namespace WarGames.Contracts.V2.Sides
{
	[Serializable]
	public class Player : IUnique<string>
	{
		public static readonly Player Empty = new();

		public Player(string id, string name, PlayerType playerType)
		{
			Id = id;
			Name = name;
			PlayerType = playerType;
		}

		public Player()
		{
			Id = string.Empty;
			Name = string.Empty;
			PlayerType = PlayerType.Unknown;
		}

		public string Id { get; set; }
		public string Name { get; set; }
		public PlayerType PlayerType { get; set; }
	}
}