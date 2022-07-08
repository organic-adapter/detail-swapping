namespace WarGames.Contracts.Game
{
	[Obsolete("We are going to a concrete model.")]
	public interface IPlayer
	{
		public string Id { get; set; }
		public string Name { get; set; }

		public PlayerType PlayerType { get; set; }
	}
}