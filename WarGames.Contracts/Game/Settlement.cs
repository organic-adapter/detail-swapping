using Map.Engine;

namespace WarGames.Contracts.Game
{
	[Serializable]
	public class Settlement : IUnique<Guid>
	{
		public Settlement()
		{
			Name = string.Empty;
			Id = Guid.NewGuid();
		}

		public Guid Id { get; set; }
		public Coord Location { get; set; }
		public string Name { get; set; }
	}
}