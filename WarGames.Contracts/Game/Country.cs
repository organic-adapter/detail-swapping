using WarGames.Contracts.Competitors;

namespace WarGames.Contracts.Game
{
	[Serializable]
	public class Country : IUnique<Guid>
	{
		public Country()
		{
			Settlements = new List<Settlement>();
			Id = Guid.NewGuid();
			Name = string.Empty;
		}

		public Guid Id { get; set; }
		public string Name { get; set; }
		public ICompetitor? Owner { get; set; }
		public List<Settlement> Settlements { get; }
	}
}