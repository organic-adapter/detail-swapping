namespace WarGames.Contracts.V2.Sides
{
	[Serializable]
	public class Player : IUnique<string>
	{
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