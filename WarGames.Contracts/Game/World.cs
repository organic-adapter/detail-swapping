namespace WarGames.Contracts.Game
{
	[Serializable]
	public class World : IUnique<Guid>
	{
		public static readonly World Empty = new World();

		public World()
		{
			Countries = new List<Country>();
		}

		public List<Country> Countries { get; set; }

		public Guid Id { get; set; }

		public List<Settlement> Settlements => Countries.SelectMany(country => country.Settlements).ToList();
	}
}