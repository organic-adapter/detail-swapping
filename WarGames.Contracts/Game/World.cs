namespace WarGames.Contracts.Game
{
	/// <summary>
	/// Our journey through the lower level elements revealed our Version 1 contracts
	/// are not JSON friendly in various ways.
	///
	/// When we persist:
	/// 1. Countries would be duplicating Settlement Data
	/// 2. The player sides duplicate the Country and Settlement data
	///   a. In turn the countries we duplicate would duplicate settlement data.
	/// 3. When loading all of this duplicate data we would have to loop through
	///		all of the data to bind the duplicates to a common reference.
	/// </summary>
	[Serializable]
	[Obsolete("Version 2 contracts inbound.")]
	public class World : IUnique<Guid>
	{
		public static readonly World Empty = new World();

		public static World Deprecating = new World();

		public World()
		{
			Id = Guid.NewGuid();
			Countries = new List<Country>();
			PlayerSides = new List<IPlayerSide>();
		}

		public List<Country> Countries { get; set; }
		public Guid Id { get; set; }
		public IEnumerable<IPlayerSide> PlayerSides { get; set; } //Deserializers cannot automatically map to any part of this.
		public List<Settlement> Settlements => Countries.SelectMany(country => country.Settlements).ToList();
	}
}