namespace WarGames.Contracts.Game
{
	[Serializable]
	public class World : IUnique<Guid>
	{
		public World()
		{
			Countries = new List<Country>();
		}

		public List<Country> Countries { get; set; }

		public Guid Id { get; set; }
	}
}