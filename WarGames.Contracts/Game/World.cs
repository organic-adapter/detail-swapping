namespace WarGames.Contracts.Game
{
	/// <summary>
	/// I made an explicit point to make this a serializable class in contracts.
	/// It declares the intent that it must be serializable.
	/// 
	/// What happens to a default serializer when you introduce interfaces?
	/// What happens to a default deserializer when you introduce interfaces?
	/// 
	/// Though this was purposeful sabotage, where it falls apart is I've mixed
	/// Interface model representations with class contracts.
	/// 
	/// Are interfaces important? Absolutely, for establishing generic boundaries 
	/// of logic that can be mocked.
	/// 
	/// How do we fix this? Coming soon in a check-in near you:
	/// 
	/// We now need to convert the interfaces to classes to allow the default serializers
	/// a chance to use these objects.
	/// </summary>
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
		public IEnumerable<IPlayerSide> PlayerSides { get; set; } //Deserializers cannot automatically map to any part of this.
		public List<Settlement> Settlements => Countries.SelectMany(country => country.Settlements).ToList();
	}
}